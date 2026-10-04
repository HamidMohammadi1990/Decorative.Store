export interface AdminUserRole {
  id: string
  userId: string
  userName: string
  roleId: string
  roleTitle: string
}

export interface CreateAdminUserRoleInput {
  userId: string
  roleId: string
}
