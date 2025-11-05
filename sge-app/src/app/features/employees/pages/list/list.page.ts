import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule, NavController, ViewWillEnter } from '@ionic/angular';
import { AlertController, ToastController } from '@ionic/angular';

import { addIcons } from 'ionicons';
import {
  add,
  arrowDown,
  arrowUp,
  createOutline,
  trashBinOutline
} from 'ionicons/icons';

import { EmployeesService } from '../../employees.service';
import { Employee } from '../../../../core/models/employee.model';

@Component({
  selector: 'app-list',
  templateUrl: './list.page.html',
  styleUrls: ['./list.page.scss'],
  standalone: true,
  imports: [IonicModule, CommonModule, FormsModule],
})
export class ListPage implements OnInit, ViewWillEnter {
  employees: Employee[] = [];
  loading = false;
  searchQuery = '';
  
  // Paginación
  currentPage = 1;
  pageSize = 10;
  totalCount = 0;
  totalPages = 0;
  
  // Ordenamiento
  orderBy = 'FullName';
  desc = false;

  constructor(
    private employeesService: EmployeesService,
    private navCtrl: NavController,
    private alertController: AlertController,
    private toastController: ToastController
  ) {
    addIcons({
      add,
      arrowDown,
      arrowUp,
      createOutline,
      trashBinOutline
    });
  }

  /**
   * Inicializa la carga de empleados al iniciar el componente.
   */
  ngOnInit() {
    this.loadEmployees();
  }

  /**
   * Carga los empleados cada vez que la vista va a entrar en primer plano.
   */
  ionViewWillEnter(): void {
    this.loadEmployees();
  }

  loadEmployees() {
    this.loading = true;
    this.employeesService.getEmployees(
      this.searchQuery || undefined,
      this.currentPage,
      this.pageSize,
      this.orderBy,
      this.desc
    ).subscribe({
      next: (result) => {
        this.employees = result.items;
        this.totalCount = result.totalCount;
        this.totalPages = result.totalPages;
        this.currentPage = result.page;
        this.loading = false;

        // Fallback: si la página actual vino vacía pero aún hay registros y no estamos en la primera,
        // retrocede una página y recarga.
        if (this.employees.length === 0 && this.totalCount > 0 && this.currentPage > 1) {
          this.currentPage = this.currentPage - 1;
          this.loadEmployees();
        }
      },
      error: (error) => {
        console.error('Error loading employees:', error);
        this.showToast('Error al cargar empleados', 'danger');
        this.loading = false;
      }
    });
  }

  onSearch(event: any) {
    this.searchQuery = event.target.value || '';
    this.currentPage = 1;
    this.loadEmployees();
  }

  onSort(field: string) {
    if (this.orderBy === field) {
      this.desc = !this.desc;
    } else {
      this.orderBy = field;
      this.desc = false;
    }
    this.loadEmployees();
  }

  onPageChange(page: number) {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.loadEmployees();
    }
  }

  onNew() {
    (document.activeElement as HTMLElement)?.blur();
    this.navCtrl.navigateForward('/employees/form');
  }

  onEdit(employee: Employee) {
    (document.activeElement as HTMLElement)?.blur();
    this.navCtrl.navigateForward(`/employees/form/${employee.id}`);
  }

  async onDelete(employee: Employee) {
    const alert = await this.alertController.create({
      header: 'Confirmar eliminación',
      message: `¿Está seguro de eliminar a ${employee.fullName}?`,
      buttons: [
        {
          text: 'Cancelar',
          role: 'cancel'
        },
        {
          text: 'Eliminar',
          role: 'destructive',
          handler: () => {
            this.deleteEmployee(employee.id);
          }
        }
      ]
    });

    await alert.present();
  }

  private deleteEmployee(id: number) {
    this.loading = true;
    this.employeesService.deleteEmployee(id).subscribe({
      next: () => {
        this.showToast('Empleado eliminado exitosamente', 'success');
        // Ajusta la página esperada antes de recargar para no quedar en una inexistente
        this.adjustPageAfterDeletion(1);
        this.loadEmployees();
      },
      error: (error) => {
        console.error('Error deleting employee:', error);
        this.showToast('Error al eliminar empleado', 'danger');
        this.loading = false;
      }
    });
  }

  /**
   * Ajusta la página después de eliminar `deletedCount` registros,
   * calculando el total y totalPages "previstos" para caer en una página válida.
   */
  private adjustPageAfterDeletion(deletedCount: number): void {
    const predictedTotal = Math.max(0, this.totalCount - deletedCount);
    const predictedPages = Math.max(1, Math.ceil(predictedTotal / this.pageSize));
    if (this.currentPage > predictedPages) {
      this.currentPage = predictedPages;
    }
  }

  private async showToast(message: string, color: string) {
    const toast = await this.toastController.create({
      message,
      duration: 3000,
      color,
      position: 'top'
    });
    await toast.present();
  }
}