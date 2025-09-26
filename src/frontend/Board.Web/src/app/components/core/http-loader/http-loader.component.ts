import { ChangeDetectorRef, Component, DestroyRef, inject, OnInit } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { NgClass, NgIf } from '@angular/common';
import { debounceTime } from 'rxjs';
import { LoaderService, LoaderState } from 'src/app/core/services/other/loader.service';

@Component({
  selector: 'app-http-loader',
  imports: [NgClass, NgIf],
  templateUrl: './http-loader.component.html',
  styleUrl: './http-loader.component.scss'
})
export class HttpLoaderComponent implements OnInit {
  private destroyRef = inject(DestroyRef);
  public show: boolean = false;

  constructor(private loaderService: LoaderService, private cdr: ChangeDetectorRef) { }

  ngOnInit() {
    this.loaderService.loaderState
      .pipe(
        debounceTime(400),
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe((state: LoaderState) => {
        this.show = state.show;
        this.cdr.detectChanges();
      });

  }
}
