import { Routes } from '@angular/router';
import { BoardComponent } from './components/board/board.component';

export const routes: Routes = [{
  path: '',
  children: [
    {
      path: '',
      component: BoardComponent,
    },
    {
      path: '**',
      redirectTo: '',
    },
  ],
}];
