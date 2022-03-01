-- 暗晦潜り

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnVC, a.PlacedOnRC, a.PutOnGC)
	ability1.SetTrigger("Trigger")
	ability1.SetCost("Cost")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsApplicable()
end

function Cost(check)
	if check then return obj.CanSB(1) and obj.CanSpecificDiscard({q.Location, l.Hand, q.Other, o.Order, q.Count, 1}) end
	obj.SoulBlast(1)
	obj.SpecificDiscard({q.Location, l.Hand, q.Other, o.Order, q.Count, 1})
end

function Activation()
	obj.Draw(1) 
	obj.AddCardValue({q.Other, o.ThisFieldID}, cs.BonusPower, 5000, p.UntilEndOfTurn)
	obj.AddCardValue({q.Other, o.ThisFieldID}, cs.BonusShield, 5000, p.UntilEndOfTurn)
end