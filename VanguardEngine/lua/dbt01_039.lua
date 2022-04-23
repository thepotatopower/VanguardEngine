-- 柩機の兵 キュビジア

function RegisterAbilities()
	-- on put
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnPut)
	ability1.SetMovedTo(l.Order, l.Player)
	ability1.SetLocation(l.VC)
	ability1.SetTrigger("Trigger")
	ability1.SetActivation("Activation")
	-- cont
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.Cont)
	ability2.SetLocation(l.RC)
	ability2.SetActivation("Cont")
end

function Trigger()
	return obj.Exists({q.Location, l.Applicable, q.Other, o.World})
end

function Activation()
	obj.Select({q.Location, l.PlayerUnits, q.Count, 1})
	obj.AddCardValue({q.Location, l.Selected}, cs.BonusPower, 5000, p.UntilEndOfTurn)
end

function Cont()
	if obj.IsPlayerTurn() and obj.IsAbyssalDarkNight() then
		obj.AddCardValue({q.Other, o.This}, cs.BonusPower, 5000, p.Continuous)
	end
end