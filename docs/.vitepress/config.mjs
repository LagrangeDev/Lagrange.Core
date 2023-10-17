import { defineConfig } from 'vitepress'
import { readdirSync, readFileSync } from 'node:fs'
import path from 'node:path'
import YAML from 'yaml'

const rootpath = new URL('../', import.meta.url).pathname

function docsToItems(dirnames) {
  return readdirSync(path.join(rootpath, ...dirnames), { withFileTypes: true })
    .filter(d => /^\d{2}-/g.test(d.name))
    .map(d => {
      if (d.name.endsWith('.md')) {
        return {
          text: /(?<=\d{2}-).*(?=\.md)/g.exec(d.name),
          link: path.join(...dirnames, d.name)
        }
      }
      if (d.isDirectory) {
        return {
          text: /(?<=\d{2}-).*/g.exec(d.name),
          items: docsToItems([...dirnames, d.name])
        }
      }
      return {}
    })
}

const sidebarItemsDocs = docsToItems(['docs'])

function tocToItems(arr) {
  const [head, tail] = arr.reduce(([head, tail], x) => {
    if (!x.href) {
      return [[...head, tail], x]
    }
    if (tail.name) {
      return [head, { ...tail, items: [...(tail.items || []), x] }]
    }
    return [[...head, x], {}]
  }, [[], {}])

  const foldArr = [...head, tail]

  return foldArr.map(obj => {
    let item = {
      text: obj.name,
    }
    if (obj.items) {
      item.items = tocToItems(obj.items)
    }
    if (obj.href) {
      item.link = `api/${obj.href}`
    }
    return item
  })
}

let apiObj;
try {
  const toc = readFileSync(path.join(rootpath, 'api', 'toc.yml'), 'utf8')
  apiObj = YAML.parse(toc)
} catch (err) {
  console.warn("未找到API文档")
  apiObj = []
}

const sidebarItemsApi = tocToItems(apiObj)

// console.log(JSON.stringify(sidebarItemsApi))

function findFirst(items) {
  if (items.length > 0) {
    if (items[0].link) {
      return items[0].link
    }
    return findFirst(items[0].items)
  }
  return '/'
}

// https://vitepress.dev/reference/site-config
export default defineConfig({
  base: "/Lagrange.Core",
  title: "Lagrange",
  description: "基于 QQNT 协议的高效率机器人库",
  head: [['link', { rel: 'icon', href: '/Lagrange.Core/favicon.ico' }]],
  themeConfig: {
    logo: '/public/apple-touch-icon.png',
    nav: [
      { text: '文档', link: findFirst(sidebarItemsDocs) },
      { text: 'API 参考', link: findFirst(sidebarItemsApi) }
    ],
    sidebar: {
      '/docs/': sidebarItemsDocs,
      '/api/': sidebarItemsApi
    }
  }
})
