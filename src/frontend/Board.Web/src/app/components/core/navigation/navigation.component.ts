import { Component, Inject, PLATFORM_ID, Output, EventEmitter } from '@angular/core';
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { TranslateService } from '@ngx-translate/core';
import { TranslateModule } from '@ngx-translate/core';
import { ProfileComponent } from '../profile/profile.component';

@Component({
  selector: 'navigation',
  standalone: true,
  imports: [
    MatButtonToggleModule,
    CommonModule,
    FormsModule,
    TranslateModule,
    ProfileComponent
  ],
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.scss'],
})
export class NavigationComponent {
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
}
