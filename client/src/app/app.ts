import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit, signal } from '@angular/core';
import { lastValueFrom, throwError } from 'rxjs';
import { Nav } from '../layout/nav/nav';
import { AccountService } from '../core/services/account-service';
import { Home } from '../features/home/home';
import { User } from '../types/user';
import { Router, RouterOutlet } from '@angular/router';
import { NgClass } from '@angular/common';

@Component({
  selector: 'app-root',
  imports: [Nav, RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.css',
})
export class App {
  protected router = inject(Router);
}
