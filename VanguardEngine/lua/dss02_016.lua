-- ディアブロスダイバー エメット

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnRC)
	ability1.SetTrigger("Trigger")
	ability1.SetCost("Cost")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsApplicable()
end

function Cost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end

function CanFullyResolve()
	return obj.IsSameZone() or obj.Exists({q.Location, l.PlayerVC, q.NameContains, obj.LoadName("Bruce")})
end

function Activation()
	obj.Select({q.Location, l.PlayerVC, q.NameContains, obj.LoadName("Bruce"), q.Count, 1})
	obj.AddCardValue({q.Location, l.Selected}, cs.BonusPower, 5000, p.UntilEndOfTurn)
	obj.AddCardValue({q.Other, o.ThisFieldID}, cs.BonusPower, 5000, p.UntilEndOfTurn)
	if obj.Exists({q.Location, l.Selected, q.Grade, 4}) then
		obj.CounterCharge(1)
		obj.SoulCharge(1)
	end
end