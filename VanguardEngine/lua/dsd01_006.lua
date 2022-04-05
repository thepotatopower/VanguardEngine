-- ヴェルリーナ

function RegisterAbilities()
	-- overDress requirement
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetOverDress("isOverDressTarget")
	-- on attack
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.OnAttack)
	ability2.SetLocation(l.RC)
	ability2.SetTrigger("Trigger")
	ability2.SetActivation("Activation")
end

function isOverDressTarget(id)
	obj.Store(id)
	return obj.Exists({q.Location, l.Stored, q.Name, obj.GetNameFromCardID("dsd01_009")})
end

function Trigger()
	return obj.Exists({q.Location, l.PlayerRC, q.Other, o.Attacking, q.Other, o.OverDress, q.Other, o.ThisFieldID}) and obj.Exists({q.Location, l.EnemyVC, q.Other, o.Attacked})
end

function Condition()
	return obj.IsOverDress()
end

function Activation()
	obj.AddCardValue({q.Location, l.PlayerUnits, q.Other, o.ThisFieldID}, cs.BonusPower, 10000, p.UntilEndOfTurn)
	if obj.CanSB(2) and obj.YesNo(obj.GetDescription(3)) then
		obj.SoulBlast(2)
		obj.ChooseRetire({q.Location, l.EnemyRC, q.Other, o.CanChoose, q.Count, 1})
	end
end