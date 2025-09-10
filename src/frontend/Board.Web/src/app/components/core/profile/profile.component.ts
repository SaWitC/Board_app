import { Component, Inject, OnInit, PLATFORM_ID } from '@angular/core';
import { CommonModule, isPlatformBrowser, NgIf } from '@angular/common';
import { RouterLink, RouterModule } from '@angular/router';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { AuthService, User } from '@auth0/auth0-angular';
import { NgSelectComponent } from '@ng-select/ng-select';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { FormsModule } from '@angular/forms';
import { UserService } from 'src/app/core/services/auth/user.service';

@Component({
  selector: 'profile',
  standalone: true,
  imports: [
    CommonModule,
    MatButtonToggleModule,
    RouterModule,
    FormsModule,
    NgSelectComponent,
    TranslateModule
],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss',
})
export class ProfileComponent implements OnInit {
  public isEditor: boolean = false;
  public userDetails: User | null | undefined = null;

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
    private userService: UserService,
    private authService: AuthService,
    @Inject(PLATFORM_ID) private platformId: Object,
    private translateService: TranslateService
  ) { }

  public ngOnInit(): void {
    if (isPlatformBrowser(this.platformId)) {
      const savedTheme = localStorage.getItem('theme');
      if (savedTheme === 'dark') {
        this.theme = 'dark';
        document.querySelector('body')?.classList.add('dark');
      } else {
        document.querySelector('body')?.classList.remove('dark');
      }

      this.selectedLanguage = localStorage.getItem('user-language') ?? 'en';
      this.translateService.use(this.selectedLanguage);
    }

    this.authService.user$.subscribe((user) => {
      this.userDetails = user;
    });
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

  public onLanguageChange(language: any) {
    this.translateService.use(language.code);
    if (isPlatformBrowser(this.platformId)) {
      localStorage.setItem('user-language', language.code);
    }
  }

  public logout() {
    this.authService.logout();
  }
}
