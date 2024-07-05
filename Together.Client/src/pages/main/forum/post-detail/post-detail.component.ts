import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '@/core/abstractions';
import { PostService } from '@/shared/services';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'together-post-detail',
  standalone: true,
  imports: [],
  templateUrl: './post-detail.component.html',
})
export class PostDetailComponent extends BaseComponent implements OnInit {
  constructor(
    private postService: PostService,
    private activatedRoute: ActivatedRoute,
  ) {
    super();
  }

  ngOnInit() {
    this.loadData();
  }

  private loadData() {}
}
