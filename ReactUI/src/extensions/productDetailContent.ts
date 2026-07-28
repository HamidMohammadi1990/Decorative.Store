import type { Locale } from '@/models/shared/locale.model'
import type { ImageAsset } from '@/models/shared/image.model'
import type { ProductSummary } from '@/models/catalog/product.model'
import type {
  ProductDetail,
  ProductFeature,
  ProductFeatureGroup,
} from '@/models/catalog/productDetail.model'

interface DetailCopy {
  description: string
  longDescription?: string[]
  highlights: string[]
  dimensions?: string
  care?: string
  deliveryNote?: string
  weight?: string
  assembly?: string
  warranty?: string
  origin?: string
  frame?: string
  extraImages?: ImageAsset[]
}

const GALLERY_POOL: ImageAsset[] = [
  { src: '/images/home/living-room.jpg', alt: 'Living room setting' },
  { src: '/images/home/bedroom.jpg', alt: 'Bedroom setting' },
  { src: '/images/home/dining.jpg', alt: 'Dining setting' },
  { src: '/images/home/velvet-sofa.jpg', alt: 'Detail view' },
  { src: '/images/home/bedroom-set.jpg', alt: 'Styled interior' },
  { src: '/images/home/new-arrivals.jpg', alt: 'Room inspiration' },
]

const VALUE_LABELS: Record<Locale, Record<string, string>> = {
  en: {
    green: 'Green',
    grey: 'Grey',
    walnut: 'Walnut',
    oak: 'Oak',
    white: 'White',
    beige: 'Beige',
    cream: 'Cream',
    natural: 'Natural',
    brass: 'Brass',
    '3-seater': '3 Seater',
    '2-seater': '2 Seater',
    armchair: 'Armchair',
    sectional: 'Sectional',
    velvet: 'Velvet',
    linen: 'Linen',
    wood: 'Wood',
    wool: 'Wool',
    ceramic: 'Ceramic',
    rattan: 'Rattan',
    metal: 'Metal',
    glass: 'Glass',
    cotton: 'Cotton',
    fabric: 'Fabric',
    'performance-fabric': 'Performance fabric',
    'living-room': 'Living room',
    bedroom: 'Bedroom',
    dining: 'Dining',
    garden: 'Garden',
    'home-office': 'Home office',
  },
  fa: {
    green: 'سبز',
    grey: 'خاکستری',
    walnut: 'گردویی',
    oak: 'بلوط',
    white: 'سفید',
    beige: 'بژ',
    cream: 'کرم',
    natural: 'طبیعی',
    brass: 'برنجی',
    '3-seater': '۳ نفره',
    '2-seater': '۲ نفره',
    armchair: 'صندلی راحتی',
    sectional: 'گوشه',
    velvet: 'مخمل',
    linen: 'کتان',
    wood: 'چوب',
    wool: 'پشم',
    ceramic: 'سرامیک',
    rattan: 'حصیری',
    metal: 'فلز',
    glass: 'شیشه',
    cotton: 'نخی',
    fabric: 'پارچه',
    'performance-fabric': 'پارچه مقاوم',
    'living-room': 'نشیمن',
    bedroom: 'خواب',
    dining: 'ناهارخوری',
    garden: 'باغ',
    'home-office': 'دفتر',
  },
}

const FEATURE_LABELS: Record<
  Locale,
  {
    dimensions: string
    color: string
    size: string
    material: string
    room: string
    weight: string
    assembly: string
    warranty: string
    origin: string
    frame: string
    availability: string
    groupGeneral: string
    groupTechnical: string
    groupCare: string
  }
> = {
  en: {
    dimensions: 'Dimensions',
    color: 'Colour',
    size: 'Size',
    material: 'Material',
    room: 'Room',
    weight: 'Weight',
    assembly: 'Assembly',
    warranty: 'Warranty',
    origin: 'Country of origin',
    frame: 'Frame',
    availability: 'Availability',
    groupGeneral: 'General',
    groupTechnical: 'Technical',
    groupCare: 'Care & delivery',
  },
  fa: {
    dimensions: 'ابعاد',
    color: 'رنگ',
    size: 'اندازه',
    material: 'جنس',
    room: 'اتاق',
    weight: 'وزن',
    assembly: 'مونتاژ',
    warranty: 'گارانتی',
    origin: 'کشور سازنده',
    frame: 'قاب',
    availability: 'موجودی',
    groupGeneral: 'مشخصات کلی',
    groupTechnical: 'مشخصات فنی',
    groupCare: 'نگهداری و ارسال',
  },
}

const DEFAULT_SPECS: Record<Locale, Pick<DetailCopy, 'weight' | 'assembly' | 'warranty' | 'origin'>> = {
  en: {
    weight: 'Varies by configuration',
    assembly: 'Professional assembly recommended',
    warranty: '1-year limited warranty',
    origin: 'Responsibly manufactured',
  },
  fa: {
    weight: 'بسته به پیکربندی متفاوت است',
    assembly: 'مونتاژ حرفه‌ای توصیه می‌شود',
    warranty: 'گارانتی محدود ۱ ساله',
    origin: 'تولید مسئولانه',
  },
}

function galleryForProduct(product: ProductSummary): ImageAsset[] {
  const images = [product.image]
  const extras = GALLERY_POOL.filter((img) => img.src !== product.image.src).slice(0, 4)
  return [...images, ...extras.map((img) => ({ ...img, alt: `${product.title} — ${img.alt}` }))]
}

function labelFor(locale: Locale, value: string) {
  return VALUE_LABELS[locale][value] ?? value.replace(/-/g, ' ')
}

function buildFeatures(
  product: ProductSummary,
  locale: Locale,
  copy: DetailCopy,
): ProductFeature[] {
  const labels = FEATURE_LABELS[locale]
  const defaults = DEFAULT_SPECS[locale]
  const features: ProductFeature[] = []

  const push = (label: string, value?: string) => {
    if (value) features.push({ label, value })
  }

  push(labels.dimensions, copy.dimensions)
  push(
    labels.color,
    product.facets.color?.map((v) => labelFor(locale, v)).join(', '),
  )
  push(
    labels.size,
    product.facets.size?.map((v) => labelFor(locale, v)).join(', '),
  )
  push(
    labels.material,
    product.facets.material?.map((v) => labelFor(locale, v)).join(', '),
  )
  push(
    labels.room,
    product.facets.room?.map((v) => labelFor(locale, v)).join(', '),
  )
  push(labels.frame, copy.frame)
  push(labels.weight, copy.weight ?? defaults.weight)
  push(labels.assembly, copy.assembly ?? defaults.assembly)
  push(labels.warranty, copy.warranty ?? defaults.warranty)
  push(labels.origin, copy.origin ?? defaults.origin)
  push(
    labels.availability,
    product.inStock
      ? locale === 'fa'
        ? 'موجود در انبار'
        : 'In stock'
      : locale === 'fa'
        ? 'سفارشی'
        : 'Made to order',
  )

  return features
}

function buildFeatureGroups(
  product: ProductSummary,
  locale: Locale,
  copy: DetailCopy,
): ProductFeatureGroup[] {
  const labels = FEATURE_LABELS[locale]
  const defaults = DEFAULT_SPECS[locale]

  const push = (list: ProductFeature[], label: string, value?: string) => {
    if (value) list.push({ label, value })
  }

  const general: ProductFeature[] = []
  push(
    general,
    labels.color,
    product.facets.color?.map((v) => labelFor(locale, v)).join('، '),
  )
  push(
    general,
    labels.size,
    product.facets.size?.map((v) => labelFor(locale, v)).join('، '),
  )
  push(
    general,
    labels.material,
    product.facets.material?.map((v) => labelFor(locale, v)).join('، '),
  )
  push(
    general,
    labels.room,
    product.facets.room?.map((v) => labelFor(locale, v)).join('، '),
  )
  push(
    general,
    labels.availability,
    product.inStock
      ? locale === 'fa'
        ? 'موجود در انبار'
        : 'In stock'
      : locale === 'fa'
        ? 'سفارشی'
        : 'Made to order',
  )

  const technical: ProductFeature[] = []
  push(technical, labels.dimensions, copy.dimensions)
  push(technical, labels.frame, copy.frame)
  push(technical, labels.weight, copy.weight ?? defaults.weight)
  push(technical, labels.assembly, copy.assembly ?? defaults.assembly)
  push(technical, labels.warranty, copy.warranty ?? defaults.warranty)
  push(technical, labels.origin, copy.origin ?? defaults.origin)

  const groups: ProductFeatureGroup[] = [
    { id: 'general', title: labels.groupGeneral, features: general },
    { id: 'technical', title: labels.groupTechnical, features: technical },
  ]

  if (copy.care) {
    groups.push({
      id: 'care',
      title: labels.groupCare,
      features: [{ label: locale === 'fa' ? 'نگهداری' : 'Care', value: copy.care }],
    })
  }

  return groups.filter((group) => group.features.length > 0)
}

function buildLongDescription(
  product: ProductSummary,
  locale: Locale,
  copy: DetailCopy,
): string[] {
  if (copy.longDescription && copy.longDescription.length > 0) {
    return copy.longDescription
  }

  const material = product.facets.material?.map((v) => labelFor(locale, v)).join(', ')
  const room = product.facets.room?.map((v) => labelFor(locale, v)).join(', ')

  if (locale === 'fa') {
    return [
      `${product.title} با تمرکز بر کیفیت ساخت و زیبایی‌شناسی مدرن طراحی شده است. هر جزئیات — از انتخاب مواد اولیه تا دوخت و مونتاژ نهایی — با دقت بررسی شده تا سال‌ها در فضای شما بدرخشد.`,
      material
        ? `این محصول از ${material} باکیفیت ساخته شده و بافت و استحکام مناسبی برای استفاده روزمره دارد. ترکیب فرم و عملکرد، آن را به انتخابی عملی و در عین حال شیک تبدیل کرده است.`
        : 'مواد اولیه با استانداردهای بالا انتخاب شده‌اند تا دوام، راحتی و ظاهر ماندگار را تضمین کنند.',
      room
        ? `برای فضای ${room} ایده‌آل است و به‌راحتی با سایر قطعات دکور هماهنگ می‌شود. می‌توانید آن را به‌تنهایی به‌عنوان نقطه کانونی اتاق قرار دهید یا در کنار مجموعه‌های مکمل دیبا گالری بچینید.`
        : 'این قطعه به‌راحتی با سبک‌های مختلف دکوراسیون هماهنگ می‌شود و فضای شما را گرم‌تر و منسجم‌تر می‌کند.',
      'تیم طراحی دیبا گالری برای راهنمایی در انتخاب رنگ، ابعاد و چیدمان در دسترس است. قبل از سفارش، ابعاد فضا و مسیر ورود محصول را اندازه‌گیری کنید تا تجربه تحویل و نصب بی‌دردسری داشته باشید.',
    ]
  }

  return [
    `${product.title} is designed with a focus on lasting quality and modern aesthetics. Every detail — from material selection to final assembly — is considered so this piece feels at home in your space for years to come.`,
    material
      ? `Crafted in ${material}, it offers a balanced combination of texture, structure, and everyday comfort. The silhouette is refined without feeling formal, making it easy to live with.`
      : 'Premium materials are chosen for durability, comfort, and a timeless look that works beyond seasonal trends.',
    room
      ? `Ideal for the ${room.toLowerCase()}, it layers beautifully with other Diba Gallery pieces. Style it as a focal point or pair it with complementary accents to complete the room.`
      : 'It pairs naturally with a wide range of interiors and helps create a cohesive, welcoming atmosphere.',
    'Our design crew can help with colour, scale, and layout advice before you order. We recommend measuring your space and delivery path to ensure a smooth arrival and setup.',
  ]
}

const COPY: Record<Locale, Record<string, DetailCopy>> = {
  en: {
    'andes-3-seater-sofa': {
      description:
        'A modern silhouette with bench-seat comfort. Clean lines, plush cushioning, and a tailored profile for contemporary living spaces.',
      longDescription: [
        'The Andes 3 Seater Sofa brings together architectural lines and sink-in comfort. Its bench-style seat creates a clean, uninterrupted surface that feels generous without overwhelming the room — ideal for apartments, open-plan living, and family spaces alike.',
        'Upholstered in rich velvet with a removable cover system, the Andes is made for real life. The solid kiln-dried hardwood frame uses reinforced joinery for long-term stability, while high-resilience foam cores recover their shape after daily use.',
        'Low-profile arms and a tailored back keep the silhouette contemporary, so the sofa pairs easily with mixed materials — think oak coffee tables, woven rugs, and brass lighting. Available in considered colourways that layer with both warm and cool palettes.',
        'For delivery, we recommend measuring hallways, stairwells, and door frames in advance. White-glove service is available in select areas, and our design specialists can advise on fabric care, cushion rotation, and layout options for your room.',
      ],
      highlights: [
        'Solid wood frame with reinforced joinery',
        'Removable velvet covers for easy care',
        'Bench-seat cushion for generous lounging',
      ],
      dimensions: 'W 220 × D 95 × H 85 cm · Seat height 45 cm',
      care: 'Vacuum regularly. Spot-clean with a damp cloth. Professional cleaning recommended annually.',
      deliveryNote: 'White-glove delivery available. Please measure doorways before ordering.',
      weight: '68 kg',
      frame: 'Kiln-dried hardwood',
      assembly: 'Legs attach in minutes; no tools required',
      warranty: '5-year frame warranty',
      origin: 'Crafted in Vietnam',
    },
    'harmony-armchair': {
      description:
        'Relaxed linen upholstery and a supportive high back — an inviting accent for reading nooks and living rooms alike.',
      longDescription: [
        'The Harmony Armchair is designed as a quiet statement piece — soft in appearance, supportive in feel. Its high back and deep seat invite you to stay longer, whether you are reading, conversing, or simply unwinding at the end of the day.',
        'Seasonal linen upholstery adds breathable texture and a relaxed, lived-in character. Feather-wrapped seat cushions provide a gentle give that settles comfortably over time, while kiln-dried hardwood legs ground the chair with a natural, warm finish.',
        'At 82 cm wide, Harmony works in compact corners, bedroom sitting areas, and larger living rooms where you want a secondary seat with personality. It complements sectional sofas, media units, and side tables without competing for attention.',
        'To preserve the fabric, rotate cushions monthly and keep the chair away from prolonged direct sunlight. In-stock units typically ship within 5–7 business days; contact our team for swatches and pairing recommendations.',
      ],
      highlights: ['Feather-wrapped seat cushions', 'Kiln-dried hardwood legs', 'New-season linen weave'],
      dimensions: 'W 82 × D 88 × H 90 cm',
      care: 'Rotate cushions monthly. Avoid direct sunlight to preserve fabric colour.',
      deliveryNote: 'In stock and ready to ship within 5–7 business days.',
      weight: '24 kg',
      frame: 'Hardwood + engineered base',
      assembly: 'Fully assembled; legs only',
      warranty: '2-year limited warranty',
      origin: 'Made in India',
    },
    default: {
      description:
        'Thoughtfully designed and crafted for everyday living. Quality materials meet timeless style.',
      highlights: [
        'Designed exclusively for Diba Gallery',
        'Responsibly sourced materials',
        'Pairs beautifully with our collections',
      ],
      deliveryNote: 'Complimentary fabric swatches available through our design crew.',
    },
  },
  fa: {
    'andes-3-seater-sofa': {
      description:
        'طراحی مدرن با راحتی نشیمنگاه یکپارچه. خطوط تمیز و بالشت‌های نرم برای فضاهای معاصر.',
      longDescription: [
        'مبل سه‌نفره آندز ترکیبی از خطوط معماری و راحتی عمیق است. نشیمنگاه یکپارچه فرم تمیزی ایجاد می‌کند و بدون شلوغ کردن فضا، حس وسعت می‌دهد — مناسب آپارتمان، نشیمن باز و خانه‌های خانوادگی.',
        'روکش مخملی با سیستم جداسازی، این مبل را برای زندگی روزمره آماده کرده است. قاب چوبی خشک‌شده با اتصالات تقویت‌شده پایداری بلندمدت دارد و هسته‌های فوم با بازگشت‌پذیری بالا پس از استفاده روزانه شکل خود را حفظ می‌کنند.',
        'دسته‌های کوتاه و پشتی ظریف، سیلوئت معاصری ساخته‌اند که با مواد مختلف — میز عسلی بلوط، فرش بافته و نورپردازی برنجی — به‌خوبی هماهنگ می‌شود.',
        'پیش از سفارش، راهروها، پله‌ها و چارچوب درب را اندازه‌گیری کنید. ارسال ویژه در برخی مناطق فعال است و تیم طراحی ما در انتخاب رنگ، نگهداری پارچه و چیدمان اتاق راهنمایی می‌کند.',
      ],
      highlights: [
        'قاب چوبی محکم با اتصالات تقویت‌شده',
        'روکش مخملی قابل جداسازی',
        'کوسن نشیمنگاه برای استراحت راحت',
      ],
      dimensions: 'عرض ۲۲۰ × عمق ۹۵ × ارتفاع ۸۵ سانتی‌متر',
      care: 'به‌طور منظم جاروبرقی بکشید. لکه‌ها را با پارچه نم‌دار پاک کنید.',
      deliveryNote: 'ارسال ویژه در دسترس است. قبل از سفارش ابعاد درب‌ها را اندازه بگیرید.',
      weight: '۶۸ کیلوگرم',
      frame: 'چوب سخت خشک‌شده',
      assembly: 'پایه‌ها در چند دقیقه نصب می‌شوند',
      warranty: 'گارانتی ۵ ساله قاب',
      origin: 'ساخت ویتنام',
    },
    'harmony-armchair': {
      description:
        'روکش کتان راحت و پشتی بلند — انتخابی دلنشین برای گوشه مطالعه و نشیمن.',
      longDescription: [
        'صندلی هارمونی به‌عنوان یک قطعه آرام اما تأثیرگذار طراحی شده — ظاهری نرم و احساسی حمایت‌کننده. پشتی بلند و نشیمن عمیق، شما را برای مطالعه، گفت‌وگو یا استراحت دعوت می‌کند.',
        'روکش کتان فصلی بافت تنفسی و حس زندگی روزمره می‌دهد. کوسن‌های پر شده نرمی ملایمی دارند که با گذشت زمان راحت‌تر می‌شود و پایه چوبی خشک‌شده پایداری طبیعی به صندلی می‌بخشد.',
        'با عرض ۸۲ سانتی‌متر، هارمونی برای گوشه‌های کوچک، فضای نشستن اتاق خواب و نشیمن‌های بزرگ مناسب است و بدون ایجاد شلوغی، کنار مبل‌های گوشه و میزهای جانبی قرار می‌گیرد.',
        'برای حفظ پارچه، کوسن‌ها را ماهانه بچرخانید و از قرار دادن در معرض نور مستقیم خورشید خودداری کنید. ارسال معمولاً ۵ تا ۷ روز کاری زمان می‌برد؛ برای نمونه پارچه و پیشنهاد ست‌کردن با تیم ما تماس بگیرید.',
      ],
      highlights: ['کوسن‌های پر شده', 'پایه چوبی خشک‌شده', 'بافت کتان فصل جدید'],
      dimensions: 'عرض ۸۲ × عمق ۸۸ × ارتفاع ۹۰ سانتی‌متر',
      care: 'کوسن‌ها را ماهانه بچرخانید. از نور مستقیم خورشید دوری کنید.',
      deliveryNote: 'موجود و آماده ارسال در ۵ تا ۷ روز کاری.',
      weight: '۲۴ کیلوگرم',
      frame: 'چوب سخت + پایه مهندسی‌شده',
      assembly: 'مونتاژ کامل؛ فقط پایه‌ها',
      warranty: 'گارانتی محدود ۲ ساله',
      origin: 'ساخت هند',
    },
    default: {
      description:
        'با وسواس طراحی و ساخته شده برای زندگی روزمره. مواد باکیفیت با استایل ماندگار.',
      highlights: [
        'طراحی انحصاری دیبا گالری',
        'مواد اولیه مسئولانه',
        'هماهنگ با سایر مجموعه‌ها',
      ],
      deliveryNote: 'نمونه پارچه رایگان از تیم طراحی در دسترس است.',
    },
  },
}

export function enrichProductDetail(
  product: ProductSummary,
  locale: Locale,
): ProductDetail {
  const localeCopy = COPY[locale]
  const copy = localeCopy[product.slug] ?? localeCopy.default

  return {
    ...product,
    description: copy.description,
    longDescription: buildLongDescription(product, locale, copy),
    highlights: copy.highlights,
    dimensions: copy.dimensions,
    care: copy.care,
    deliveryNote: copy.deliveryNote,
    warranty: copy.warranty,
    images: galleryForProduct(product),
    features: buildFeatures(product, locale, copy),
    featureGroups: buildFeatureGroups(product, locale, copy),
  }
}
