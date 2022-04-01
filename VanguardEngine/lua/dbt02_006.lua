-- 極光戦姫 ペリオ・ターコイズ

function RegisterAbilities()
	-- cont
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.RC)
	ability1.SetActivation("Cont")
	-- on place
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.PlacedOnRCFromPrison)
	ability2.SetLocation(l.FrontRowRC)
	ability2.SetTrigger("Trigger")
	ability2.SetCondition("Condition")
	ability2.SetActivation("Activation")
end

function Cont()
	if obj.IsPlayerTurn() and obj.GetNumberOf({q.Location, l.PlayerPrisoners}) >= 2 then
		obj.AddCardValue({q.Other, o.This}, cs.BonusPower, 5000, p.Continuous)
	end
end

function Trigger()
	return obj.Exists({q.Location, l.EnemyUnits, q.Other, o.Applicable, q.Other, o.Enemy})
end

function Condition()
	return obj.Exists({q.Location, l.EnemyUnits, q.Other, o.Applicable, q.Other, o.Enemy})
end

function Activation()
	obj.AddCardValue({q.Location, l.EnemyUnits, q.Other, o.Applicable, q.Other, o.Enemy}, cs.BonusPower, -5000, p.UntilEndOfTurn)
end