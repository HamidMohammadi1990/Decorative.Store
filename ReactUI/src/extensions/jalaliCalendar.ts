/**
 * Jalali (Persian) calendar utilities — stores dates as ISO Gregorian YYYY-MM-DD.
 */

const PERSIAN_MONTHS = [
  'فروردین',
  'اردیبهشت',
  'خرداد',
  'تیر',
  'مرداد',
  'شهریور',
  'مهر',
  'آبان',
  'آذر',
  'دی',
  'بهمن',
  'اسفند',
]

const PERSIAN_WEEKDAYS = ['ش', 'ی', 'د', 'س', 'چ', 'پ', 'ج']

export function toPersianDigits(value: string | number): string {
  return String(value).replace(/\d/g, (d) => '۰۱۲۳۴۵۶۷۸۹'[Number(d)])
}

export function gregorianToJalali(gy: number, gm: number, gd: number): [number, number, number] {
  const gDaysInMonth = [0, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31]
  let jy = gy <= 1600 ? 0 : 979
  let gy2 = gy - (gy <= 1600 ? 621 : 1600)
  const days =
    365 * gy2 +
    Math.floor((gy2 + 3) / 4) -
    Math.floor((gy2 + 99) / 100) +
    Math.floor((gy2 + 399) / 400) -
    80 +
    gd +
    gDaysInMonth.slice(0, gm).reduce((a, b) => a + b, 0)

  jy += 33 * Math.floor(days / 12053)
  let rem = days % 12053
  jy += 4 * Math.floor(rem / 1461)
  rem %= 1461
  jy += Math.floor((rem - 1) / 365)
  if (rem > 365) rem = (rem - 1) % 365

  const jm = rem < 186 ? 1 + Math.floor(rem / 31) : 7 + Math.floor((rem - 186) / 30)
  const jd = 1 + (rem < 186 ? rem % 31 : (rem - 186) % 30)
  return [jy, jm, jd]
}

export function jalaliToGregorian(jy: number, jm: number, jd: number): [number, number, number] {
  let gy = jy <= 979 ? 621 : 1600
  jy -= jy <= 979 ? 0 : 979
  const days =
    365 * jy +
    Math.floor(jy / 33) * 8 +
    Math.floor(((jy % 33) + 3) / 4) +
    78 +
    jd +
    (jm < 7 ? (jm - 1) * 31 : (jm - 7) * 30 + 186)

  gy += 400 * Math.floor(days / 146097)
  let rem = days % 146097
  if (rem >= 36525) {
    gy += 100 * Math.floor(--rem / 36524)
    rem %= 36524
    if (rem >= 365) rem++
  }
  gy += 4 * Math.floor(rem / 1461)
  rem %= 1461
  gy += Math.floor((rem - 1) / 365)
  if (rem > 0) rem = (rem - 1) % 365

  const salA = [0, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31]
  let gm = 0
  for (gm = 1; gm <= 12 && rem >= salA[gm]; gm++) rem -= salA[gm]
  return [gy, gm, rem + 1]
}

export function parseIsoDate(iso: string): [number, number, number] | null {
  const match = /^(\d{4})-(\d{2})-(\d{2})$/.exec(iso)
  if (!match) return null
  return [Number(match[1]), Number(match[2]), Number(match[3])]
}

export function formatIsoDate(y: number, m: number, d: number): string {
  return `${y}-${String(m).padStart(2, '0')}-${String(d).padStart(2, '0')}`
}

export function isoToJalaliLabel(iso: string): string {
  const parts = parseIsoDate(iso)
  if (!parts) return ''
  const [jy, jm, jd] = gregorianToJalali(parts[0], parts[1], parts[2])
  return `${toPersianDigits(jy)}/${toPersianDigits(String(jm).padStart(2, '0'))}/${toPersianDigits(String(jd).padStart(2, '0'))}`
}

export function jalaliMonthLength(jy: number, jm: number): number {
  if (jm <= 6) return 31
  if (jm <= 11) return 30
  const isLeap = [1, 5, 9, 13, 17, 22, 26, 30].includes(((jy + 38) * 31) % 128)
  return isLeap ? 30 : 29
}

export function getTodayIso(): string {
  const now = new Date()
  return formatIsoDate(now.getFullYear(), now.getMonth() + 1, now.getDate())
}

export function getTodayJalali(): [number, number, number] {
  const now = new Date()
  return gregorianToJalali(now.getFullYear(), now.getMonth() + 1, now.getDate())
}

export function buildJalaliMonthGrid(
  jy: number,
  jm: number,
): { day: number; iso: string }[] {
  const length = jalaliMonthLength(jy, jm)
  const cells: { day: number; iso: string }[] = []
  for (let day = 1; day <= length; day++) {
    const [gy, gm, gd] = jalaliToGregorian(jy, jm, day)
    cells.push({ day, iso: formatIsoDate(gy, gm, gd) })
  }
  return cells
}

export { PERSIAN_MONTHS, PERSIAN_WEEKDAYS }
