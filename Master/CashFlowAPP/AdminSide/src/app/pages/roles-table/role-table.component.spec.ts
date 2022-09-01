﻿

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RolesTableComponent } from './role-table.component';

describe('#UserTableComponent', () => {
  let component: RolesTableComponent;
  let fixture: ComponentFixture<RolesTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RolesTableComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RolesTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
