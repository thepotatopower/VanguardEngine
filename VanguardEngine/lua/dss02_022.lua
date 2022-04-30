-- グラビディア・マルト

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnAttack)
	ability1.SetLocation(l.RC)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetActivation("Activation")
	ability1.SetProperty(p.NotMandatory)
end

function Trigger()
	return obj.IsAttackingUnit()
end

function Condition()
	return obj.CanPutIntoOrderZone({q.Location, l.Hand, q.Other, o.Meteorite, q.Count, 1})
end

function Activation()
	obj.Store(obj.ChoosePutIntoOrderZone({q.Location, l.Hand, q.Other, o.Meteorite, q.Count, 1, q.Min, 0}))
	if obj.Exists({q.Location, l.Stored}) then
		obj.AddCardValue({q.Other, o.ThisFieldID}, cs.BonusPower, 5000, p.UntilEndOfTurn)
	end
end