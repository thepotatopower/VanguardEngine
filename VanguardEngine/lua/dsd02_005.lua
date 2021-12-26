-- 烈震空拳の時空巨兵

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnRC)
	ability1.SetTrigger("Trigger")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsApplicable()
end

function Activation()
	obj.SoulCharge(1)
	if obj.InFinalRush() and obj.CanCB(1) and obj.PayAdditionalCost() then
		obj.CounterBlast(1)
		obj.AddCardValue({q.Location, l.PlayerUnits, q.Other, o.ThisFieldID}, cs.BonusPower, 15000, p.UntilEndOfTurn)
	end
end