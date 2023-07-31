/// <reference types="Cypress" />

describe('Testando rotas', () => {
  beforeEach(function () {
    cy.visit({
      url: 'http://127.0.0.1:5000/',
      method: 'GET',
    })
  })

  it('Teste Ajuste de Marcação', () => {
    cy.get('[href="/AdjustWorkShift"] > .q-btn__content').click()
    cy.get('h6').should(($p) => expect($p).to.contain("Ajuste de Marcação"))
  })

  it('Teste Ajuste de Marcação pelo menu', () => {
    cy.get('.q-btn__content > .q-icon').click()
    cy.get('[href="/AdjustWorkShift"] > .q-item__section--main').click()
    cy.get('h6').should(($p) => expect($p).to.contain("Ajuste de Marcação"))
  })

  it('Teste Cobertura de Pontos', () => {
    cy.get('[href="/CoverWorkShift"] > .q-btn__content').click()
    cy.get('h6').should(($p) => expect($p).to.contain("Cobertura de Posto"))
  })

  it('Teste Cobertura de Pontos pelo menu', () => {
    cy.get('.q-btn__content > .q-icon').click()
    cy.get('[href="/CoverWorkShift"] > .q-item__section--main').click()
    cy.get('h6').should(($p) => expect($p).to.contain("Cobertura de Posto"))
  })

  it('Teste Lançar Ausência', () => {
    cy.get('[href="/Absence"] > .q-btn__content').click()
    cy.get('h6').should(($p) => expect($p).to.contain("Lançar Ausência"))
  })

  it('Teste Lançar Ausência pelo menu', () => {
    cy.get('.q-btn__content > .q-icon').click()
    cy.get('[href="/Absence"] > .q-item__section--main').click()
    cy.get('h6').should(($p) => expect($p).to.contain("Lançar Ausência"))
  })

  it('Testando voltar página inicial', () => {
    cy.get('[href="/CoverWorkShift"] > .q-btn__content').click()
    cy.get('.q-toolbar__title').click()
    cy.get('.text-subtitle2').should(($p) => expect($p).to.contain("Escolha uma das opções"))
  })

  it('Testando voltar página inicial pelo menu', () => {
    cy.get('.q-btn__content > .q-icon').click()
    cy.get('.q-list > [href="/CoverWorkShift"]').click()
    cy.get('.q-toolbar > .q-btn > .q-btn__content').click()
    cy.get('[href="/"] > .q-item__section--side').click()
    cy.get('.text-subtitle2').should(($p) => expect($p).to.contain("Escolha uma das opções"))
  })
})
