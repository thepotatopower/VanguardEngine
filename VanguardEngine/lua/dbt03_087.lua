-- ブリッツ・インタラプター

function RegisterAbilities()
	-- on put
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PutOnGC)
	ability1.SetTrigger("Trigger")
	ability1.SetCanFullyResolve("CanFullyResolve")
	ability1.SetCost("Cost")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsApplicable()
end

function CanFullyResolve()
	return obj.IsSameZone()
end

function Cost(check)
	if check then return obj.CanSB(1) and obj.CanSpecificDiscard({q.Location, l.Hand, q.Other, o.SetOrder, q.Count, 1}) end
	obj.SoulBlast(1)
	obj.SpecificDiscard({q.Location, l.Hand, q.Other, o.SetOrder, q.Count, 1})
end

function Activation()
	obj.AddCardValue({q.Location, l.PlayerUnits, q.Other, o.ThisFieldID}, cs.BonusShield, 15000, p.UntilEndOfBattle)
end