-- 天閃の騎士 エクレシア

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnAttack)
	ability1.SetLocation(l.RC)
	ability1.SetProperty(p.OncePerTurn)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsApplicable()
end

function Condition()
	return obj.Exists({q.Location, l.PlayerUnits, q.Grade, 3, q.Other, o.SameColumn})
end

function Activation()
	obj.AddCardValue({q.Other, o.ThisFieldID}, cs.BonusGrade, 1, p.UntilEndOfTurn)
	obj.AddCardValue({q.Other, o.ThisFieldID}, cs.BonusPower, 5000, p.UntilEndOfTurn)
end