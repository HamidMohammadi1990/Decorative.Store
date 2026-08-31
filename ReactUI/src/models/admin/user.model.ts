export type AdminUserGender = 1 | 2

export interface AdminUser {
  id: string
  userName: string
  firstName: string
  lastName: string
  email: string
  phoneNumber: string
  gender: AdminUserGender
  emailConfirmed: boolean
  phoneNumberConfirmed: boolean
  loginPermission: boolean
  isActive: boolean
  lastLoginDateOnUtc: string | null
}

export interface CreateAdminUserInput {
  userName: string
  firstName: string
  lastName: string
  email: string
  phoneNumber: string
  password: string
  gender: AdminUserGender
}

export interface UpdateAdminUserInput {
  id: string
  userName: string
  firstName: string
  lastName: string
  email: string
  phoneNumber: string
  password?: string
  gender: AdminUserGender
  isActive: boolean
  loginPermission: boolean
}
