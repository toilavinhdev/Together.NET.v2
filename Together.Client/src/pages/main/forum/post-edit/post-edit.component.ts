import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '@/core/abstractions';
import {
  FormsModule,
  ReactiveFormsModule,
  UntypedFormBuilder,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';
import { AsyncPipe, JsonPipe, NgIf } from '@angular/common';
import { InputTextModule } from 'primeng/inputtext';
import { DropdownModule } from 'primeng/dropdown';
import { ForumService, PostService, PrefixService } from '@/shared/services';
import { map, Observable, take, takeUntil } from 'rxjs';
import { Button } from 'primeng/button';
import { getErrorMessage, isGUID, markFormDirty } from '@/shared/utilities';
import { SelectItem, SelectItemGroup } from 'primeng/api';
import { ActivatedRoute, Router } from '@angular/router';
import { EditorModule } from 'primeng/editor';

@Component({
  selector: 'together-post-edit',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    JsonPipe,
    InputTextModule,
    DropdownModule,
    AsyncPipe,
    Button,
    FormsModule,
    EditorModule,
    NgIf,
  ],
  templateUrl: './post-edit.component.html',
})
export class PostEditComponent extends BaseComponent implements OnInit {
  formType: 'create' | 'update' = 'create';

  form!: UntypedFormGroup;

  forums$!: Observable<SelectItemGroup[]>;

  loading = false;

  constructor(
    private formBuilder: UntypedFormBuilder,
    protected prefixService: PrefixService,
    protected forumService: ForumService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private postService: PostService,
  ) {
    super();
  }

  ngOnInit() {
    this.commonService.breadcrumb$.next([
      {
        title: 'Tạo bài viết',
      },
    ]);
    this.buildForm();
    this.routerTracking();
    this.loadPrefixes();
    this.loadTopics();
    this.forums$ = this.forumService.forums$.pipe(
      map((data) =>
        data.map(
          (forum) =>
            ({
              label: forum.name,
              items: forum.topics?.map(
                (topic) =>
                  ({
                    label: topic.name,
                    value: topic.id,
                  }) as SelectItem,
              ),
            }) as SelectItemGroup,
        ),
      ),
    );
  }

  onSubmit() {
    if (this.form.invalid) {
      markFormDirty(this.form);
      return;
    }
    this.loading = true;
    if (this.formType === 'create') {
      this.postService
        .createPost(this.form.value)
        .pipe(takeUntil(this.destroy$))
        .subscribe({
          next: () => {
            this.loading = false;
            this.commonService.toast$.next({
              type: 'success',
              message: 'Tạo bài viết thành công',
            });
            this.commonService.navigateToTopic(this.form.get('topicId')?.value);
          },
          error: (err) => {
            this.loading = false;
            this.commonService.toast$.next({
              type: 'error',
              message: getErrorMessage(err),
            });
          },
        });
    }
  }

  private routerTracking() {
    this.activatedRoute.paramMap
      .pipe(takeUntil(this.destroy$))
      .subscribe((paramMap) => {
        const topicId = paramMap.get('topicId');
        if (!topicId || !isGUID(topicId)) return;
        this.form.get('topicId')?.setValue(topicId);
      });
    this.form
      .get('topicId')
      ?.valueChanges.pipe(takeUntil(this.destroy$))
      .subscribe((forumId) => {
        this.router.navigate(['/', 'topics', forumId, 'create-post']).then();
      });
  }

  private buildForm() {
    this.form = this.formBuilder.group({
      id: [null],
      topicId: [null, [Validators.required]],
      prefixId: [null],
      title: [null, [Validators.required]],
      body: [null, [Validators.required]],
    });
  }

  private loadPrefixes() {
    this.prefixService.prefixes$.pipe(take(1)).subscribe((exists) => {
      if (exists.length > 0) return;
      this.prefixService
        .listPrefix()
        .pipe(takeUntil(this.destroy$))
        .subscribe({
          next: (data) => {
            this.prefixService.prefixes$.next(data);
          },
          error: (err) => {
            this.commonService.toast$.next({
              type: 'error',
              message: getErrorMessage(err),
            });
          },
        });
    });
  }

  private loadTopics() {
    this.forumService.forums$.pipe(take(1)).subscribe((exists) => {
      if (exists.length > 0) return;
      this.forumService
        .listForum()
        .pipe(takeUntil(this.destroy$))
        .subscribe({
          next: (data) => {
            this.forumService.forums$.next(data);
          },
          error: (err) => {
            this.commonService.toast$.next({
              type: 'error',
              message: getErrorMessage(err),
            });
          },
        });
    });
  }
}
