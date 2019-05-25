# -*- coding: utf-8 -*-

import pygame as pg
from fsm import *
from config import *

class Over(BaseState):
    def __init__(self):
        super(Over, self).__init__()
        self.title = self.font.render(
            "Game Over", True, pg.Color("white"), 36)
        self.titles = self.font.render(
            "Parabéns, você perdeu(!)", True, pg.Color("white"), 36)
        self.aperta = self.font.render(
            "Pressione ENTER para sair", True, pg.Color("white"), 36)
        self.titleRect = self.title.get_rect()
        self.titlesRect = self.title.get_rect()
        self.titlesRect.center = (WIDTH / 2, (HEIGHT / 2) + 50)
        self.titleRect.center = (WIDTH / 2, HEIGHT / 2)
        self.next_state = "GAMEPLAY"


    def startup(self, persist):
        self.score = self.font.render("{} pontos".format(persist['score']), True, pg.Color("red"))
        self.scoreRect = self.score.get_rect()
        self.scoreRect.center = (WIDTH / 2, (HEIGHT / 2) + 95)

    def get_event(self, event):
        if event.type == pg.KEYDOWN:
            if event.key == pg.K_ENTER:
                self.done = True

    def draw(self, surface):
        surface.fill(pg.Color("black"))
        surface.blit(self.title, self.titleRect)
        surface.blit(self.titles, self.titlesRect)
        surface.blit(self.aperta, (WIDTH / 2, (HEIGHT / 2) + 90))
        surface.blit(self.score, self.scoreRect)