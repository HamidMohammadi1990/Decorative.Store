export interface CategoryTreeProduct {
  id: string
  title: string
  slug: string
}

export interface CategoryTreeSubCategory {
  id: string
  title: string
  slug: string
  products: CategoryTreeProduct[]
}

export interface CategoryTreeItem {
  id: string
  title: string
  slug: string
  subCategories: CategoryTreeSubCategory[]
}
