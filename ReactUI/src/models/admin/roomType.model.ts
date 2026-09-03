export interface AdminRoomType {
  id: string
  code: string
  title: string | null
  imageFileName: string
  imageUrl: string
  priority: number
  isActive: boolean
  languageId: number | null
}

export interface RoomTypeItem {
  id: string
  code: string
  title: string
  imageUrl: string
  priority: number
}

export interface CreateAdminRoomTypeInput {
  languageId: number
  code: string
  title: string
  imageFileName: string
  priority: number
}

export interface UpdateAdminRoomTypeInput extends CreateAdminRoomTypeInput {
  id: string
  isActive: boolean
}
