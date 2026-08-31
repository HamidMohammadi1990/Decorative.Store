export interface AdminPermission {
  id: string
  title: string
  url: string
  nameSpace: string | null
  parentId: string | null
  levelTypeId: number
  levelTypeTitle: string
  priority: number
  isActive: boolean
}
