declare module 'vue-cropper' {
  import { DefineComponent } from 'vue'
  
  export const VueCropper: DefineComponent<{
    img: string
    outputSize?: number
    outputType?: string
    info?: boolean
    canScale?: boolean
    autoCrop?: boolean
    autoCropWidth?: number
    autoCropHeight?: number
    fixed?: boolean
    fixedNumber?: [number, number]
    full?: boolean
    fixedBox?: boolean
    canMove?: boolean
    canMoveBox?: boolean
    original?: boolean
    centerBox?: boolean
    high?: boolean
    infoTrue?: boolean
    maxImgSize?: number
    enlarge?: number
    mode?: 'contain' | 'cover' | 'default'
  }>
} 