export interface AdminRole {
  id: string
  title: string
  isActive: boolean
}

export interface CreateAdminRoleInput {
  title: string
}

export interface UpdateAdminRoleInput {
  id: string
  title: string
  isActive: boolean
}
