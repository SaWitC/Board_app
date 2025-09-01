import { Component, EventEmitter, Output, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.scss'
})
export class SidebarComponent {
  @Output() closeSidebar = new EventEmitter<void>();
  @Output() openSidebar = new EventEmitter<void>();
  @Input() isOpen = false;

  constructor(private router: Router) {
    console.log('Sidebar component initialized');
  }

  onOpenSidebar(): void {
    this.openSidebar.emit();
  }

  onMyProjectsClick(): void {
    this.router.navigate(['/boards']);

    this.closeSidebar.emit();
  }

  onInfoClick(): void {
    this.router.navigate(['/info']);
    this.closeSidebar.emit();
  }

  onCloseClick(): void {
    this.closeSidebar.emit();
  }
}
