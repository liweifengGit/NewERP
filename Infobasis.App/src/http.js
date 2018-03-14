import axios from 'axios'
import store from './main'
import * as types from './store/types'
import router from './router'
import ibconfig from '../config/ibconfig.js'

// axios 配置
axios.defaults.timeout = 5000
axios.defaults.baseURL = ibconfig.APIRoot

// http request 拦截器
axios.interceptors.request.use(
    config => {
      if (store.state.token) {
        config.headers.Authorization = `Bearer ${store.state.token}`
      }
      return config
    },
    err => {
      return Promise.reject(err)
    })

// http response 拦截器
axios.interceptors.response.use(
    response => {
      return response
    },
    error => {
      if (error.response) {
        switch (error.response.status) {
          case 401:
          // 401 清除token信息并跳转到登录页面
            store.commit(types.LOGOUT)
            router.replace({
              path: 'login',
              query: {redirect: router.currentRoute.fullPath}
            })
        }
      }
      // console.log(JSON.stringify(error));//console : Error: Request failed with status code 402
      if (error.response) {
        return Promise.reject(error.response.data)
      } else {
        return Promise.reject(error)
      }
    })

export default axios
