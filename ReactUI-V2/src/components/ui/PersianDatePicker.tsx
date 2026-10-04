import { useEffect, useMemo, useRef, useState } from 'react'
import { useTranslation } from 'react-i18next'
import {
  buildJalaliMonthGrid,
  getTodayIso,
  getTodayJalali,
  gregorianToJalali,
  isoToJalaliLabel,
  PERSIAN_MONTHS,
  parseIsoDate,
  toPersianDigits,
} from '@/extensions/jalaliCalendar'

interface PersianDatePickerProps {
  value: string
  onChange: (isoDate: string) => void
  placeholder?: string
  className?: string
}

export function PersianDatePicker({
  value,
  onChange,
  placeholder,
  className = '',
}: PersianDatePickerProps) {
  const { t } = useTranslation()
  const rootRef = useRef<HTMLDivElement>(null)
  const [open, setOpen] = useState(false)

  const initialJalali = useMemo(() => {
    const parts = parseIsoDate(value)
    if (parts) return gregorianToJalali(parts[0], parts[1], parts[2])
    return getTodayJalali()
  }, [value])

  const [viewYear, setViewYear] = useState(initialJalali[0])
  const [viewMonth, setViewMonth] = useState(initialJalali[1])

  useEffect(() => {
    if (!open) return
    const parts = parseIsoDate(value)
    if (parts) {
      const [jy, jm] = gregorianToJalali(parts[0], parts[1], parts[2])
      setViewYear(jy)
      setViewMonth(jm)
    }
  }, [open, value])

  useEffect(() => {
    if (!open) return
    const onDocClick = (event: MouseEvent) => {
      if (!rootRef.current?.contains(event.target as Node)) setOpen(false)
    }
    document.addEventListener('mousedown', onDocClick)
    return () => document.removeEventListener('mousedown', onDocClick)
  }, [open])

  const monthCells = buildJalaliMonthGrid(viewYear, viewMonth)
  const display = value ? isoToJalaliLabel(value) : ''

  const selectDay = (iso: string) => {
    onChange(iso)
    setOpen(false)
  }

  const shiftMonth = (delta: number) => {
    let month = viewMonth + delta
    let year = viewYear
    while (month < 1) {
      month += 12
      year -= 1
    }
    while (month > 12) {
      month -= 12
      year += 1
    }
    setViewMonth(month)
    setViewYear(year)
  }

  return (
    <div ref={rootRef} className={`relative ${className}`}>
      <button
        type="button"
        onClick={() => setOpen((prev) => !prev)}
        className="flex w-full items-center justify-between rounded-sm border border-border bg-surface px-3 py-2.5 text-sm text-text outline-none transition-colors hover:border-warm/50 focus:border-warm focus:ring-2 focus:ring-warm/15"
      >
        <span className={display ? 'font-medium' : 'text-text-muted'}>
          {display || placeholder || t('dashboard.profileCompletion.datePlaceholder')}
        </span>
        <span className="text-warm" aria-hidden>
          📅
        </span>
      </button>

      {open && (
        <div className="absolute z-50 mt-2 w-full min-w-[18rem] rounded-sm border border-border bg-surface p-3 shadow-lg ring-1 ring-black/5">
          <div className="mb-3 flex items-center justify-between gap-2">
            <button
              type="button"
              onClick={() => shiftMonth(-1)}
              className="rounded-sm px-2 py-1 text-sm text-text hover:bg-surface-muted"
              aria-label={t('dashboard.profileCompletion.prevMonth')}
            >
              ‹
            </button>
            <p className="text-sm font-semibold text-text">
              {PERSIAN_MONTHS[viewMonth - 1]} {toPersianDigits(viewYear)}
            </p>
            <button
              type="button"
              onClick={() => shiftMonth(1)}
              className="rounded-sm px-2 py-1 text-sm text-text hover:bg-surface-muted"
              aria-label={t('dashboard.profileCompletion.nextMonth')}
            >
              ›
            </button>
          </div>

          <div className="mb-2 grid grid-cols-7 gap-1 text-center text-[10px] font-semibold text-text-muted">
            {['ش', 'ی', 'د', 'س', 'چ', 'پ', 'ج'].map((day) => (
              <span key={day}>{day}</span>
            ))}
          </div>

          <div className="grid grid-cols-7 gap-1">
            {monthCells.map(({ day, iso }) => {
              const selected = value === iso
              const isToday = iso === getTodayIso()
              return (
                <button
                  key={iso}
                  type="button"
                  onClick={() => selectDay(iso)}
                  className={`aspect-square rounded-sm text-xs font-medium transition-colors ${
                    selected
                      ? 'bg-warm text-warm-text shadow-sm'
                      : isToday
                        ? 'bg-warm-soft text-warm ring-1 ring-warm/30'
                        : 'text-text hover:bg-surface-muted'
                  }`}
                >
                  {toPersianDigits(day)}
                </button>
              )
            })}
          </div>

          <div className="mt-3 flex items-center justify-between gap-2 border-t border-border pt-3">
            <button
              type="button"
              className="text-xs font-medium text-warm hover:underline"
              onClick={() => selectDay(getTodayIso())}
            >
              {t('dashboard.profileCompletion.today')}
            </button>
            {value && (
              <button
                type="button"
                className="text-xs text-text-muted hover:text-text"
                onClick={() => {
                  onChange('')
                  setOpen(false)
                }}
              >
                {t('dashboard.profileCompletion.clearDate')}
              </button>
            )}
          </div>
        </div>
      )}
    </div>
  )
}
