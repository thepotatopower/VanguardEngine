-- みんなで寄り道 エルヴィ

function RegisterAbilities()
	-- cont
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.FrontRowRC)
	ability1.SetActivation("Cont")
	-- on attack hits
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.OnAttackHits)
	ability2.SetLocation(l.RC)
	ability2.SetTrigger("OnAttackHitsTrigger")
	ability2.SetCondition("OnAttackHitsCondition")
	ability2.SetActivation("OnAttackHits")
	ability2.SetProperty(p.NotMandatory)
end

function Cont()
	obj.AddCardState("ContFilter", {l.Units}, cs.Resist, p.Continuous)
end

function ContFilter(id) 
	return obj.IsPlayer(id) and obj.IsRearguard(id) and not obj.NameIs(id, obj.GetName())
end

function OnAttackHitsTrigger()
	return obj.IsAttackingUnit() and obj.Exists({q.Location, l.EnemyVC, q.Other, o.Attacked})
end

function OnAttackHitsCondition()
	return obj.Exists({q.Location, l.Damage, q.Other, o.FaceDown})
end

function OnAttackHits()
	if obj.YesNo(obj.GetDescription(3)) then
		obj.CounterCharge(1)
	end
end