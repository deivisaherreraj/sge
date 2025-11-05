import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { IonicModule, NavController, ToastController } from '@ionic/angular';

import { forkJoin, of } from 'rxjs';
import { map, switchMap } from 'rxjs/operators';

import { EmployeesService } from '../../employees.service';
import { Department, Employee } from '../../../../core/models/employee.model';

@Component({
  selector: 'app-form',
  templateUrl: './form.page.html',
  styleUrls: ['./form.page.scss'],
  standalone: true,
  imports: [IonicModule, CommonModule, FormsModule, ReactiveFormsModule]
})
export class FormPage implements OnInit {
  form!: FormGroup;
  departments: Department[] = [];
  loading = false;
  isEditMode = false;
  employeeId?: number;

  constructor(
    private fb: FormBuilder,
    private employeesService: EmployeesService,
    private navCtrl: NavController,
    private route: ActivatedRoute,
    private toastController: ToastController
  ) {
    this.initForm();
  }

  ngOnInit() {
    this.loading = true;
    this.route.paramMap.pipe(
      map(params => params.get('id')),
      switchMap(idStr => {
        const id = idStr ? +idStr : null;
        this.isEditMode = !!id;
        this.employeeId = id ?? undefined;

        return forkJoin({
          departments: this.employeesService.getDepartments(),
          employee: id ? this.employeesService.getEmployee(id) : of(null)
        });
      })
    ).subscribe({
      next: ({ departments, employee }) => {
        this.departments = departments ?? [];
        if (employee) {
          this.patchEmployee(employee);
        }
        this.loading = false;
      },
      error: (err) => {
        console.error('Error inicializando formulario:', err);
        this.showToast('Error al cargar datos', 'danger');
        this.loading = false;
      }
    });
  }

  private initForm() {
    this.form = this.fb.group({
      fullName: ['', [Validators.required, Validators.maxLength(120)]],
      role: ['', [Validators.required, Validators.maxLength(80)]],
      hireDate: ['', Validators.required],
      salary: [0, [Validators.required, Validators.min(0)]],
      departmentId: ['', Validators.required]
    });
  }

  private patchEmployee(employee: Employee) {
    // Encontrar el ID del departamento correspondiente
    const depId = this.departments.find(d => d.name === employee.department)?.id ?? '';
    // Rellenar el formulario con los datos del empleado
    this.form.patchValue({
      fullName: employee.fullName,
      role: employee.role,
      hireDate: employee.hireDate?.includes('T') ? employee.hireDate.split('T')[0] : employee.hireDate,
      salary: employee.salary,
      departmentId: depId
    });
  }

  onSubmit() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.loading = true;
    const formValue = this.form.value;

    if (this.isEditMode && this.employeeId) {
      this.employeesService.updateEmployee(this.employeeId, formValue).subscribe({
        next: () => {
          this.showToast('Empleado actualizado exitosamente', 'success');
          this.navCtrl.navigateForward('/employees/list');
        },
        error: (error) => {
          console.error('Error updating employee:', error);
          this.showToast('Error al actualizar empleado', 'danger');
          this.loading = false;
        }
      });
    } else {
      this.employeesService.createEmployee(formValue).subscribe({
        next: () => {
          this.showToast('Empleado creado exitosamente', 'success');
          this.navCtrl.navigateForward('/employees/list');
        },
        error: (error) => {
          console.error('Error creating employee:', error);
          this.showToast('Error al crear empleado', 'danger');
          this.loading = false;
        }
      });
    }
  }

  onCancel() {    
    this.navCtrl.navigateForward('/employees/list');
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

  get fullName() { return this.form.get('fullName'); }
  get role() { return this.form.get('role'); }
  get hireDate() { return this.form.get('hireDate'); }
  get salary() { return this.form.get('salary'); }
  get departmentId() { return this.form.get('departmentId'); }
}