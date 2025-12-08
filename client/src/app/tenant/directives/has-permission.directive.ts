import { Directive, Input, OnDestroy, OnInit, TemplateRef, ViewContainerRef } from '@angular/core';
import { Permission } from '../constants/permissions';
import { Subject, takeUntil } from 'rxjs';
import { PermissionService } from '../services/permission.service';

@Directive({
  selector: '[appHasPermission], [appHasAnyPermissions], [appHasAllPermissions]'
})
export class HasPermissionDirective implements OnInit, OnDestroy {
  private permission?: Permission;
  private permissions?: Permission[];
  private checkType: 'single' | 'any' | 'all' = 'single';
  private destroy$ = new Subject<void>();
  private initialized = false;

  @Input()
  set appHasPermission(permission: Permission) {
    this.permission = permission;
    this.checkType = 'single';
    if (this.initialized) {
      this.updateView();
    }
  }

  @Input()
  set appHasAnyPermissions(permissions: Permission[]) {
    this.permissions = permissions;
    this.checkType = 'any';
    if (this.initialized) {
      this.updateView();
    }
  }

  @Input()
  set appHasAllPermissions(permissions: Permission[]) {
    this.permissions = permissions;
    this.checkType = 'all';
    if (this.initialized) {
      this.updateView();
    }
  }

  constructor(
    private templateRef: TemplateRef<any>,
    private viewContainer: ViewContainerRef,
    private permissionService: PermissionService
  ) { }

  ngOnInit(): void {
    this.initialized = true;
    this.updateView();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private updateView(): void {
    this.viewContainer.clear();

    let checkPermission$;

    switch (this.checkType) {
      case 'single':
        if (!this.permission) {
          console.warn('appHasPermission usado sem fornecer permissão');
          return;
        }
        checkPermission$ = this.permissionService.hasPermission(this.permission);
        break;

      case 'any':
        if (!this.permissions || this.permissions.length === 0) {
          console.warn('appHasAnyPermissions usado sem fornecer permissões');
          return;
        }
        checkPermission$ = this.permissionService.hasAnyPermission(this.permissions);
        break;

      case 'all':
        if (!this.permissions || this.permissions.length === 0) {
          console.warn('appHasAllPermissions usado sem fornecer permissões');
          return;
        }
        checkPermission$ = this.permissionService.hasAllPermission(this.permissions);
        break;

      default:
        return;
    }

    checkPermission$
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (hasPermission) => {
          this.viewContainer.clear();
          if (hasPermission) {
            this.viewContainer.createEmbeddedView(this.templateRef);
          }
        },
        error: (err) => {
          console.error('Erro ao verificar permissão:', err);
          this.viewContainer.clear();
        }
      });
  }
}
