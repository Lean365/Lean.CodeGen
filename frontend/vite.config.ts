import { fileURLToPath, URL } from 'node:url'
import { defineConfig, loadEnv } from 'vite'
import vue from '@vitejs/plugin-vue'
import vueJsx from '@vitejs/plugin-vue-jsx'
import Components from 'unplugin-vue-components/vite'
import { AntDesignVueResolver } from 'unplugin-vue-components/resolvers'
import AutoImport from 'unplugin-auto-import/vite'

// https://vitejs.dev/config/
export default defineConfig(({ mode }) => {
  const env = loadEnv(mode, process.cwd())
  return {
    plugins: [
      vue(),
      vueJsx(),
      // 自动导入 Vue 相关函数
      AutoImport({
        imports: [
          'vue',
          'vue-router',
          'vue-i18n',
          '@vueuse/core',
          'pinia'
        ],
        dts: './src/auto-imports.d.ts',
        dirs: [
          './src/composables',
          './src/stores',
          './src/utils'
        ],
        vueTemplate: true
      }),
      // 自动导入组件
      Components({
        resolvers: [
          AntDesignVueResolver({
            importStyle: 'less',
            resolveIcons: true
          })
        ],
        dts: './src/components.d.ts',
        dirs: ['./src/components']
      })
    ],
    server: {
      port: 5153,
      strictPort: true,
      proxy: {
        '/api': {
          target: env.VITE_API_BASE_URL,
          changeOrigin: true,
          secure: false
        }
      }
    },
    resolve: {
      alias: {
        '@': fileURLToPath(new URL('./src', import.meta.url)),
        '@assets': fileURLToPath(new URL('./src/assets', import.meta.url)),
        '@components': fileURLToPath(new URL('./src/components', import.meta.url)),
        '@views': fileURLToPath(new URL('./src/views', import.meta.url)),
        '@layouts': fileURLToPath(new URL('./src/layouts', import.meta.url)),
        '@stores': fileURLToPath(new URL('./src/stores', import.meta.url)),
        '@utils': fileURLToPath(new URL('./src/utils', import.meta.url)),
        '@api': fileURLToPath(new URL('./src/api', import.meta.url)),
        '@router': fileURLToPath(new URL('./src/router', import.meta.url)),
        '@styles': fileURLToPath(new URL('./src/styles', import.meta.url)),
        '@plugins': fileURLToPath(new URL('./src/plugins', import.meta.url)),
        '@locales': fileURLToPath(new URL('./src/locales', import.meta.url)),
        '@types': fileURLToPath(new URL('./src/types', import.meta.url)),
        '@hooks': fileURLToPath(new URL('./src/hooks', import.meta.url)),
        '@constants': fileURLToPath(new URL('./src/constants', import.meta.url))
      }
    },
    css: {
      preprocessorOptions: {
        less: {
          javascriptEnabled: true,
          modifyVars: {
            'border-radius-base': '2px'
          }
        }
      }
    }
  }
})
