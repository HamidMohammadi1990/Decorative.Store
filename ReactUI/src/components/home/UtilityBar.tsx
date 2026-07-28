import type { UtilityBar as UtilityBarModel } from '@/models/home/utilityBar.model'
import { LanguageSwitcher } from '@/components/layout/LanguageSwitcher'
import { ThemeSwitcher } from '@/components/layout/ThemeSwitcher'
import { Container } from '@/components/ui/Container'
import { TextLink } from '@/components/ui/TextLink'

interface UtilityBarProps {
  data: UtilityBarModel
}

export function UtilityBar({ data }: UtilityBarProps) {
  return (
    <div className="hidden border-b border-border text-xs text-text-muted md:block">
      <Container className="flex items-center justify-between py-2">
        <div className="flex items-center gap-4">
          <a href={data.phoneHref} className="hover:text-text">
            {data.phoneLabel}
          </a>
          {data.links.map((link) => (
            <TextLink key={link.href} link={link} className="hover:text-text" />
          ))}
        </div>
        <div className="flex items-center gap-2">
          <ThemeSwitcher />
          <LanguageSwitcher />
        </div>
      </Container>
    </div>
  )
}
