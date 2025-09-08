import { Component, Inject, PLATFORM_ID, Output, EventEmitter } from '@angular/core';
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { AuthService } from '@auth0/auth0-angular';
import { TranslateService } from '@ngx-translate/core';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'navigation',
  standalone: true,
  imports: [
    MatButtonToggleModule,
    CommonModule,
    FormsModule,
    TranslateModule,
  ],
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.scss'],
})
export class NavigationComponent {
  //  @Output() openSidebar = new EventEmitter<void>();

  public selectedLanguage = 'en';
  public theme: string = 'light';
  public languages = [
    {
      description: 'Русский',
      code: 'ru',
    },
    {
      description: 'English',
      code: 'en',
    },
  ];

  constructor(
    @Inject(PLATFORM_ID) private platformId: Object,
    public auth: AuthService,
    private translateService: TranslateService
  ) {}

  ngOnInit(): void {
    if (isPlatformBrowser(this.platformId)) {
      const savedTheme = localStorage.getItem('theme');
      if (savedTheme === 'dark') {
        this.theme = 'dark';
        document.querySelector('body')?.classList.add('dark');
      }
      this.selectedLanguage = localStorage.getItem('user-language') ?? 'en';
      this.translateService.use(this.selectedLanguage);
    }
  }

  public changeTheme(): void {
    if (isPlatformBrowser(this.platformId)) {
      const body = document.querySelector('body');
      if (body) {
        const isDarkSelected = body.classList.toggle('dark');
        this.theme = isDarkSelected ? 'dark' : 'light';
        localStorage.setItem('theme', this.theme);
      }
    }
  }

  // public onOpenSidebar(): void {
  //    this.openSidebar.emit();
  // }

  public onLanguageChange(language: any) {
    this.translateService.use(language.code);
    if (isPlatformBrowser(this.platformId)) {
      localStorage.setItem('user-language', language.code);
    }
  }
}
