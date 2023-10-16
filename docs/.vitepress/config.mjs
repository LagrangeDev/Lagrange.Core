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


const toc = readFileSync(path.join(rootpath, 'api', 'toc.yml'), 'utf8')

const apiObj = YAML.parse(toc)


function tocToItems(arr) {
  if (arr.length > 1 && !arr[0].href) {
    return tocToItems([{ ...arr[0], items: arr.slice(1) }])
  }
  return arr.map(obj => {
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

const sidebarItemsApi = tocToItems(apiObj)


function findFirst(items) {
  return items[0]?.link || findFirst(items[0].items) || '/'
}

// https://vitepress.dev/reference/site-config
export default defineConfig({
  title: "Lagrange",
  description: "基于 QQNT 协议的高效率机器人库",
  themeConfig: {
    // https://vitepress.dev/reference/default-theme-config
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
