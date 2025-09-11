import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavigationComponent } from './components/core/navigation/navigation.component';
import { SidebarComponent } from './components/core/sidebar/sidebar.component';
import { RouterOutlet } from '@angular/router';
import { AuthService } from '@auth0/auth0-angular';
import { UserService } from './core/services/auth/user.service';
import { tap } from 'rxjs';
import { jwtDecode } from 'jwt-decode';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss'],
    standalone: true,
    imports: [CommonModule, NavigationComponent, SidebarComponent, RouterOutlet]
})
export class AppComponent implements OnInit {
  title = 'BoardAppClient';
  sidebarOpen = false;
  public isAuthenticated = false;


  onOpenSidebar(): void {
    this.sidebarOpen = true;
  }

  onCloseSidebar(): void {
    this.sidebarOpen = false;
  }
  constructor(private authService: AuthService, private userService: UserService) {}

  ngOnInit(): void {
    this.authService.isAuthenticated$
      .pipe(
        tap((result) => {
          this.isAuthenticated = result;
          this.authService.getAccessTokenSilently().subscribe(token => {
            if (token) {
              const decodedToken: any = jwtDecode(token);
              if (decodedToken.permissions) {
                this.userService.emitPermissionsChanged(decodedToken.permissions);
              }
              this.userService.setCurrentUserEmail(decodedToken.email);
            }
          });
        })
      )
      .subscribe();
  }
}
