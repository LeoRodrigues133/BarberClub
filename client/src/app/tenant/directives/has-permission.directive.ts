import { Directive, Input, OnDestroy, OnInit, TemplateRef, ViewContainerRef } from '@angular/core';
import { Permission } from '../constants/permissions';
import { Subject, takeUntil } from 'rxjs';
import { PermissionService } from '../services/permission.service';

@Directive({
  selector: '[appHasPermission]'
})
export class HasPermissionDirective implements OnInit, OnDestroy {
  private permission!: Permission;
  private destroy$ = new Subject<void>();

  @Input()
  set appHasPermission(permission: Permission) {
    this.permission = permission;
  }


  constructor(
    private templateRef: TemplateRef<any>,
    private viewContainer: ViewContainerRef,
    private permissionService: PermissionService
  ) { }

  ngOnInit(): void {
    this.updateView();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private updateView(): void {
    this.permissionService.hasPermission(this.permission)
      .pipe(takeUntil(this.destroy$))
      .subscribe(hasPermission => {
        this.viewContainer.clear();
        if (hasPermission)
          this.viewContainer.createEmbeddedView(this.templateRef);
        else
          this.viewContainer.clear()
      })
  }
}
