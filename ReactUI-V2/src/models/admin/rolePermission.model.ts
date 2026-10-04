export interface AdminRolePermission {
  id: string
  roleId: string
  roleTitle: string
  permissionId: string
  permissionTitle: string
}

export interface CreateAdminRolePermissionInput {
  roleId: string
  permissionId: string
}
