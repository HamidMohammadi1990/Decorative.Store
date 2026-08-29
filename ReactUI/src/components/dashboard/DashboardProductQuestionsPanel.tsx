import { useCallback, useEffect, useMemo, useState } from 'react'
import { Link, useSearchParams } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import type { AdminProductListItem, AdminProductQuestion } from '@/models/admin/catalog.model'
import { DashboardPageHeader } from '@/components/dashboard/DashboardPageHeader'
import { DashboardEmptyState } from '@/components/dashboard/DashboardEmptyState'
import { ProductQuestionsIcon } from '@/components/dashboard/DashboardIcons'
import {
  AdminField,
  adminInputClass,
  resolveAdminMutationError,
} from '@/components/dashboard/admin/adminFormShared'
import type { ProductPanelEmbedProps } from '@/components/dashboard/admin/productPanelEmbed'
import { embeddedProductLabel } from '@/components/dashboard/admin/productPanelEmbed'
import { Button } from '@/components/ui/Button'
import { InlineLoading } from '@/components/ui/Spinner'
import { useCurrentLanguageId } from '@/hooks/useCurrentLanguageId'
import { adminProductQuestionService } from '@/services/adminProductQuestionService'
import { adminProductService } from '@/services/adminProductService'
import { useUserStore } from '@/stores/userStore'

export function DashboardProductQuestionsPanel({
  embedded = false,
  productId: fixedProductId,
  productTitle: fixedProductTitle,
  productCode: fixedProductCode,
}: ProductPanelEmbedProps = {}) {
  const { t } = useTranslation()
  const [searchParams, setSearchParams] = useSearchParams()
  const accessToken = useUserStore((s) => s.accessToken)
  const { languageId, locale, loading: languageLoading } = useCurrentLanguageId()

  const productIdFromSource = embedded
    ? (fixedProductId ?? '')
    : (searchParams.get('productId') ?? '')

  const [items, setItems] = useState<AdminProductQuestion[]>([])
  const [products, setProducts] = useState<AdminProductListItem[]>([])
  const [loading, setLoading] = useState(true)
  const [saving, setSaving] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [filterProductId, setFilterProductId] = useState(productIdFromSource)
  const [filterStatus, setFilterStatus] = useState('')
  const [answerDrafts, setAnswerDrafts] = useState<Record<string, string>>({})

  const selectedProduct = useMemo(() => {
    if (embedded && fixedProductId) {
      return {
        id: fixedProductId,
        title: fixedProductTitle ?? '',
        productCode: fixedProductCode ?? '',
      } as AdminProductListItem
    }
    return products.find((p) => p.id === filterProductId) ?? null
  }, [embedded, fixedProductCode, fixedProductId, fixedProductTitle, filterProductId, products])

  useEffect(() => {
    if (!embedded) setFilterProductId(productIdFromSource)
  }, [embedded, productIdFromSource])

  useEffect(() => {
    if (embedded && fixedProductId) setFilterProductId(fixedProductId)
  }, [embedded, fixedProductId])

  const load = useCallback(async () => {
    if (!accessToken || accessToken === 'mock-access-token') {
      setError(t('dashboard.productQuestions.authRequired'))
      setLoading(false)
      return
    }

    const productIdForQuery = embedded ? (fixedProductId ?? filterProductId) : filterProductId

    setLoading(true)
    setError(null)
    try {
      if (!embedded) {
        const productResult = await adminProductService.getAll(accessToken, locale, {
          pageSize: 200,
          languageId: languageId ?? undefined,
        })
        setProducts(productResult.items)
      }

      const questionResult = await adminProductQuestionService.getAll(accessToken, locale, {
        pageSize: 200,
        productId: productIdForQuery || null,
        isActive:
          filterStatus === 'active' ? true : filterStatus === 'inactive' ? false : null,
      })
      setItems(questionResult.items)
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.productQuestions.loadFailed')))
      setItems([])
    } finally {
      setLoading(false)
    }
  }, [
    accessToken,
    embedded,
    fixedProductId,
    filterProductId,
    filterStatus,
    languageId,
    locale,
    t,
  ])

  useEffect(() => {
    if (!languageLoading) void load()
  }, [languageLoading, load])

  useEffect(() => {
    if (embedded) return
    if (!productIdFromSource && products.length > 0 && !filterProductId) {
      const firstId = products[0].id
      setFilterProductId(firstId)
      setSearchParams({ productId: firstId })
    }
  }, [embedded, filterProductId, productIdFromSource, products, setSearchParams])

  const handleProductFilterChange = (id: string) => {
    setFilterProductId(id)
    if (id) setSearchParams({ productId: id })
    else setSearchParams({})
  }

  const handleChangeStatus = async (id: string, isActive: boolean) => {
    if (!accessToken || accessToken === 'mock-access-token') return

    setSaving(true)
    setError(null)
    try {
      await adminProductQuestionService.changeStatus(accessToken, locale, id, isActive)
      await load()
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.productQuestions.statusFailed')))
    } finally {
      setSaving(false)
    }
  }

  const handleAnswer = async (id: string) => {
    if (!accessToken || accessToken === 'mock-access-token') return

    const answer = answerDrafts[id]?.trim()
    if (!answer) return

    setSaving(true)
    setError(null)
    try {
      await adminProductQuestionService.answer(accessToken, locale, id, answer)
      setAnswerDrafts((prev) => {
        const next = { ...prev }
        delete next[id]
        return next
      })
      await load()
    } catch (err) {
      setError(resolveAdminMutationError(err, t('dashboard.productQuestions.answerFailed')))
    } finally {
      setSaving(false)
    }
  }

  return (
    <div>
      {!embedded && (
        <DashboardPageHeader
          title={t('dashboard.productQuestions.title')}
          description={t('dashboard.productQuestions.description')}
          icon={<ProductQuestionsIcon size={22} />}
          action={
            <Link
              to="/account/dashboard/products"
              className="inline-flex items-center justify-center rounded-sm border border-border-strong px-4 py-2 text-sm font-medium text-text transition-colors hover:bg-surface-muted"
            >
              {t('dashboard.productQuestions.backToProducts')}
            </Link>
          }
        />
      )}

      {embedded && (
        <p className="mb-4 text-sm font-medium text-text">
          {embeddedProductLabel(fixedProductTitle, fixedProductCode)}
        </p>
      )}

      <div className={`mb-4 grid gap-4 ${embedded ? '' : 'sm:grid-cols-2'}`}>
        {!embedded && (
          <AdminField label={t('dashboard.productQuestions.fieldProduct')}>
            <select
              value={filterProductId}
              onChange={(e) => handleProductFilterChange(e.target.value)}
              className={adminInputClass}
            >
              <option value="">{t('dashboard.productQuestions.allProducts')}</option>
              {products.map((product) => (
                <option key={product.id} value={product.id}>
                  {product.title} ({product.productCode})
                </option>
              ))}
            </select>
          </AdminField>
        )}

        <AdminField label={t('dashboard.productQuestions.filterStatus')}>
          <select
            value={filterStatus}
            onChange={(e) => setFilterStatus(e.target.value)}
            className={adminInputClass}
          >
            <option value="">{t('dashboard.productQuestions.allStatuses')}</option>
            <option value="active">{t('dashboard.productQuestions.statusActive')}</option>
            <option value="inactive">{t('dashboard.productQuestions.statusInactive')}</option>
          </select>
        </AdminField>
      </div>

      {selectedProduct && (
        <p className="mb-4 text-xs text-text-muted">
          {t('dashboard.productQuestions.selectedProduct', {
            title: selectedProduct.title,
            code: selectedProduct.productCode,
          })}
        </p>
      )}

      {loading || languageLoading ? (
        <div className="flex justify-center py-10">
          <InlineLoading label={t('dashboard.productQuestions.loading')} />
        </div>
      ) : products.length === 0 && !embedded ? (
        <DashboardEmptyState
          icon={<ProductQuestionsIcon size={28} />}
          title={t('dashboard.productQuestions.noProductsTitle')}
          message={t('dashboard.productQuestions.noProductsMessage')}
        />
      ) : error && items.length === 0 ? (
        <DashboardEmptyState
          icon={<ProductQuestionsIcon size={28} />}
          title={t('dashboard.productQuestions.loadFailedTitle')}
          message={error}
          action={
            <Button variant="secondary" onClick={() => void load()}>
              {t('dashboard.productQuestions.retry')}
            </Button>
          }
        />
      ) : items.length === 0 ? (
        <DashboardEmptyState
          icon={<ProductQuestionsIcon size={28} />}
          title={t('dashboard.productQuestions.emptyTitle')}
          message={t('dashboard.productQuestions.emptyMessage')}
        />
      ) : (
        <div className="space-y-3">
          {error && <p className="text-sm text-sale">{error}</p>}
          <p className="text-xs text-text-muted">
            {t('dashboard.productQuestions.itemCount', { count: items.length })}
          </p>
          <ul className="divide-y divide-border rounded-sm border border-border">
            {items.map((item) => (
              <li key={item.id} className="px-4 py-4 sm:px-5">
                <div className="flex flex-wrap items-start justify-between gap-3">
                  <div className="min-w-0 flex-1">
                    <p className="text-sm font-medium text-text">{item.question}</p>
                    <p className="mt-1 text-xs text-text-muted">
                      {item.askerName || t('dashboard.productQuestions.unknownAuthor')}
                      {!filterProductId && item.productTitle ? ` · ${item.productTitle}` : ''}
                    </p>
                  </div>
                  <div className="flex flex-wrap items-center gap-2">
                    <span
                      className={`rounded-sm px-2 py-0.5 text-[10px] font-semibold uppercase tracking-wide ${
                        item.isActive
                          ? 'bg-warm-soft text-warm'
                          : 'bg-surface-muted text-text-muted'
                      }`}
                    >
                      {item.isActive
                        ? t('dashboard.productQuestions.statusActive')
                        : t('dashboard.productQuestions.statusInactive')}
                    </span>
                    {item.isActive ? (
                      <Button
                        variant="ghost"
                        className="py-1.5 text-xs text-sale hover:bg-sale/10"
                        onClick={() => void handleChangeStatus(item.id, false)}
                        disabled={saving}
                      >
                        {t('dashboard.productQuestions.deactivate')}
                      </Button>
                    ) : (
                      <Button
                        variant="secondary"
                        className="py-1.5 text-xs"
                        onClick={() => void handleChangeStatus(item.id, true)}
                        disabled={saving}
                      >
                        {t('dashboard.productQuestions.activate')}
                      </Button>
                    )}
                  </div>
                </div>

                {item.answer ? (
                  <div className="mt-3 rounded-md bg-surface-muted px-3 py-2.5">
                    <p className="text-xs font-medium text-text-muted">
                      {t('dashboard.productQuestions.answerLabel')}
                      {item.answeredByName ? ` · ${item.answeredByName}` : ''}
                    </p>
                    <p className="mt-1 text-sm leading-relaxed text-text">{item.answer}</p>
                  </div>
                ) : (
                  <div className="mt-3">
                    <label className="text-xs font-medium text-text-muted">
                      {t('dashboard.productQuestions.answerPlaceholder')}
                    </label>
                    <textarea
                      value={answerDrafts[item.id] ?? ''}
                      onChange={(e) =>
                        setAnswerDrafts((prev) => ({ ...prev, [item.id]: e.target.value }))
                      }
                      rows={3}
                      className="mt-1.5 block w-full rounded-sm border border-border bg-surface px-3 py-2 text-sm text-text outline-none focus:border-warm"
                      placeholder={t('dashboard.productQuestions.answerPlaceholder')}
                    />
                    <Button
                      variant="warm"
                      className="mt-2 py-1.5 text-xs"
                      disabled={saving || !answerDrafts[item.id]?.trim()}
                      onClick={() => void handleAnswer(item.id)}
                    >
                      {t('dashboard.productQuestions.submitAnswer')}
                    </Button>
                  </div>
                )}
              </li>
            ))}
          </ul>
        </div>
      )}
    </div>
  )
}
