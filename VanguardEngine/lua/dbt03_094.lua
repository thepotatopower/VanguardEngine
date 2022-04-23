-- ニートネス・メテオシャワー

function RegisterAbilities()
	-- to order
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnPut)
	ability1.SetMovedTo(l.Order, l.Player)
	ability1.SetTrigger("ToOrderTrigger")
	ability1.SetActivation("ToOrderActivation")
	-- to drop
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.OnPut)
	ability2.SetMovedFrom(l.Order, l.Player)
	ability2.SetMovedTo(l.Drop)
	ability2.SetTrigger("ToDropTrigger")
	ability2.SetCondition("ToDropCondition")
	ability2.SetActivation("ToDropActivation")
end

function ToOrderTrigger()
	return obj.IsApplicable()
end

function ToOrderActivation()
	if obj.YesNo(obj.GetDescription(3)) then
		obj.SoulCharge(1)
	end
	if obj.SourceIsVanguardAbility() and obj.YesNo(obj.GetDescription(4)) then
		obj.Draw(1)
	end
end

function ToDropTrigger()
	return obj.IsApplicable()
end

function ToDropCondition()
	return obj.Exists({q.Location, l.EnemyRC, q.Other, o.CanChoose})
end

function ToDropActivation()
	obj.ChooseRetire({q.Location, l.EnemyRC, q.Other, o.CanChoose, q.Count, 1})
end