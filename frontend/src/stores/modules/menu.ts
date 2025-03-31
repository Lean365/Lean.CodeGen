import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import type { Ref, ComputedRef } from 'vue'
import { getCurrentUserMenu } from '@/api/identity/menu'
import type { LeanMenuDto, LeanMenuCreateDto } from '@/types/identity/menu'
import type { RouteRecordRaw } from 'vue-router'
import router from '@/router'
import { generateRoutes } from '@/utils/route'
import type { AxiosError } from 'axios'
import { useI18n } from 'vue-i18n'
import { useRouter } from 'vue-router'

interface MenuState {
  menuList: Ref<LeanMenuDto[]>
  routes: Ref<RouteRecordRaw[]>
  isCollapse: Ref<boolean>
  fetchUserMenu: () => Promise<LeanMenuDto[]>
  getRoutes: ComputedRef<RouteRecordRaw[]>
  getMenuList: ComputedRef<LeanMenuDto[]>
  toggleCollapse: () => void
  currentPath: Ref<string>
  openKeys: Ref<string[]>
  selectedKeys: Ref<string[]>
  getMenuTitle: (menu: LeanMenuDto) => string
  findMenuByPath: (path: string, menus: LeanMenuCreateDto[]) => LeanMenuCreateDto | null
  findParentMenu: (path: string, menus: LeanMenuCreateDto[]) => LeanMenuCreateDto | null
  handleMenuClick: (menu: LeanMenuCreateDto) => Promise<void>
}

export const useMenuStore = defineStore('menu', (): MenuState => {
  const menuList = ref<LeanMenuDto[]>([])
  const routes = ref<RouteRecordRaw[]>([])
  const retryCount = ref(0)
  const maxRetries = 3
  const isCollapse = ref(false)
  const isFetching = ref(false)
  const currentPath = ref('')
  const openKeys = ref<string[]>([])
  const selectedKeys = ref<string[]>([])

  // 获取用户菜单
  const fetchUserMenu = async () => {
    // 如果正在获取菜单，直接返回
    if (isFetching.value) {
      return menuList.value
    }

    try {
      isFetching.value = true
      // console.log('开始获取用户菜单...')
      // console.log('当前重试次数:', retryCount.value)
      const { data } = await getCurrentUserMenu()
      // console.log('获取到的菜单数据:', data)
      
      // 如果获取到的菜单为空，抛出错误
      if (!data || data.length === 0) {
        throw new Error('获取到的菜单数据为空')
      }

      menuList.value = data
      // console.log('菜单数据已更新到store')
      await generateRoutes()
      // console.log('路由已生成')
      await addRoutes()
      // console.log('路由已添加到router')
      retryCount.value = 0 // 重置重试次数
      return data
    } catch (error: unknown) {
      const axiosError = error as AxiosError
      // console.error('获取用户菜单失败:', error)
      // console.log('错误详情:', {
      //   message: axiosError.message,
      //   status: axiosError.response?.status,
      //   data: axiosError.response?.data
      // })
      if (retryCount.value < maxRetries) {
        retryCount.value++
        // console.log(`准备第${retryCount.value}次重试...`)
        return await fetchUserMenu()
      } else {
        throw new Error('获取用户菜单失败，已达到最大重试次数')
      }
    } finally {
      isFetching.value = false
    }
  }

  // 获取路由列表
  const getRoutes = computed(() => routes.value)

  // 获取菜单列表
  const getMenuList = computed(() => menuList.value)

  // 生成路由
  const generateRoutes = async () => {
    routes.value = menuList.value.map(menu => generateRoute(menu))
  }

  // 生成单个路由
  const generateRoute = (menu: LeanMenuDto): RouteRecordRaw => {
    // console.log('生成路由配置:', {
    //   path: menu.path,
    //   name: menu.menuName,
    //   component: menu.component,
    //   isTopLevel: !menu.path.includes('/')
    // });

    const route: RouteRecordRaw = {
      path: menu.path,
      name: menu.menuName,
      meta: {
        title: menu.menuName,
        icon: menu.icon,
        isCache: true,
        transKey: menu.transKey,
        permissions: menu.perms,
        isTopLevel: !menu.path.includes('/')
      },
      component: menu.component ? () => {
        // 处理特殊组件
        if (menu.component === 'Layout') {
          return import('@/layouts/default/index.vue')
        }
        // 处理普通视图组件
        try {
          // 移除开头的斜杠并规范化路径
          const componentPath = menu.component.startsWith('/') ? menu.component.slice(1) : menu.component
          //console.log('Loading component:', componentPath)
          
          // 对于子菜单，尝试加载对应的组件
          return new Promise((resolve, reject) => {
            // 使用相对路径
            const importPath = `../../views/${componentPath}.vue`
            // console.log('Importing component from:', importPath)
            
            // 使用动态导入
            import(/* @vite-ignore */ importPath)
              .then(resolve)
              .catch((error) => {
                console.error('Failed to load component:', componentPath, error)
                // 如果加载失败，返回一个空的组件
                resolve({
                  default: {
                    render() {
                      return h('div', '404 Not Found')
                    }
                  }
                })
              })
          })
        } catch (error) {
          console.error('Failed to load component:', menu.component, error)
          // 如果加载失败，返回一个空的组件
          return {
            default: {
              render() {
                return h('div', '404 Not Found')
              }
            }
          }
        }
      } : undefined,
      children: menu.children?.map(child => generateRoute(child)) || []
    }

    return route
  }

  // 添加路由
  const addRoutes = async () => {
    try {
      // console.log('开始添加路由...');
      const menus = menuList.value;
      console.log('获取到的菜单数据:', menus);

      // 先添加顶级菜单路由
      const topLevelMenus = menus.filter((menu: LeanMenuDto) => !menu.path.includes('/'));
      console.log('顶级菜单:', topLevelMenus);

      for (const menu of topLevelMenus) {
        // console.log('处理顶级菜单:', {
        //   path: menu.path,
        //   name: menu.menuName,
        //   component: menu.component
        // });

        // 先添加顶级路由
        const topRoute = {
          path: menu.path,
          name: menu.menuName,
          component: () => import('@/layouts/default/index.vue'),
          meta: {
            title: menu.menuName,
            icon: menu.icon,
            permissions: menu.perms,
            isTopLevel: true
          },
          children: []
        };
        // console.log('添加顶级路由:', topRoute);
        router.addRoute(topRoute);

        // 再添加子路由
        if (menu.children && menu.children.length > 0) {
          // console.log('处理顶级菜单的子路由:', {
          //   parentPath: menu.path,
          //   childrenCount: menu.children.length
          // });

          for (const child of menu.children) {
            const childRoute = generateRoute(child);
            // console.log('添加子路由:', {
            //   parentPath: menu.path,
            //   childRoute
            // });
            router.addRoute(menu.path, childRoute);
          }
        }
      }

      // 添加其他路由
      const otherMenus = menus.filter((menu: LeanMenuDto) => menu.path.includes('/'));
      //console.log('其他菜单:', otherMenus);

      for (const menu of otherMenus) {
        const route = generateRoute(menu);
        //console.log('添加其他路由:', route);
        router.addRoute(route);
      }

      console.log('路由添加完成');
    } catch (error) {
      console.error('添加路由失败:', error);
    }
  }

  // 切换菜单折叠状态
  const toggleCollapse = () => {
    isCollapse.value = !isCollapse.value
  }

  // 获取菜单标题
  const getMenuTitle = (menu: LeanMenuDto): string => {
    try {
      // 如果没有transKey，则使用menuName作为默认值
      return menu.menuName
    } catch (error) {
      return menu.menuName
    }
  }

  // 根据路径查找菜单
  const findMenuByPath = (path: string, menus: LeanMenuCreateDto[]): LeanMenuCreateDto | null => {
    for (const menu of menus) {
      if (menu.path === path) {
        return menu
      }
      if (menu.children) {
        const found = findMenuByPath(path, menu.children)
        if (found) {
          return found
        }
      }
    }
    return null
  }

  // 查找父级菜单
  const findParentMenu = (path: string, menus: LeanMenuCreateDto[]): LeanMenuCreateDto | null => {
    for (const menu of menus) {
      if (menu.children) {
        const found = menu.children.find((child: LeanMenuCreateDto) => child.path === path)
        if (found) {
          return menu
        }
        const parent = findParentMenu(path, menu.children)
        if (parent) {
          return parent
        }
      }
    }
    return null
  }

  // 处理菜单点击
  const handleMenuClick = async (menu: LeanMenuCreateDto) => {
    if (menu.path) {
      currentPath.value = menu.path
      selectedKeys.value = [menu.path]
      await router.push(menu.path)
    }
  }

  // 构建菜单树
  const buildMenuTree = (routes: RouteRecordRaw[]): LeanMenuCreateDto[] => {
    return routes
      .filter(route => {
        // 过滤掉登录页和404页面
        if (route.path === '/login' || route.path === '/:pathMatch(.*)*') {
          return false
        }
        // 过滤掉meta.menu为false的路由
        if (route.meta?.menu === false) {
          return false
        }
        return true
      })
      .map(route => {
        // 移除开头的斜杠，将其他斜杠替换为点号
        const key = route.path.replace(/^\//, '').replace(/\//g, '.')
        const transKey = `menu.${key}`
        const menu: LeanMenuCreateDto = {
          menuName: route.meta?.title as string || route.path,
          parentId: 0,
          path: route.path,
          // 根据路径层级决定组件类型
          component: route.path === '/' ? 'Layout' : `${route.path.substring(1)}/index`,
          isFrame: 0,
          isCached: 0,
          visible: 0,
          menuStatus: 0,
          menuType: route.path === '/' ? 0 : 1, // 0-目录，1-菜单
          icon: route.meta?.icon as string,
          transKey: transKey,
          perms: route.path,
          isBuiltin: 0,
          children: route.children ? buildMenuTree(route.children) : undefined
        }
        return menu
      })
  }

  // 菜单列表
  const menuItems = computed(() => {
    const routes = router.getRoutes()
    return buildMenuTree(routes)
  })

  return {
    menuList,
    routes,
    fetchUserMenu,
    getRoutes,
    getMenuList,
    isCollapse,
    toggleCollapse,
    currentPath,
    openKeys,
    selectedKeys,
    getMenuTitle,
    findMenuByPath,
    findParentMenu,
    handleMenuClick
  }
}) 