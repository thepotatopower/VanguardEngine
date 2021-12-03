-- 極光戦姫 シャイラー・ゼニス

function RegisterAbilities()
	-- on place
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnRC)
	ability1.SetTrigger("Trigger")
	ability1.SetCost("Cost")
	ability1.SetCanFullyResolve("CanFullyResolve")
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
	return obj.HasPrison() and obj.Exists({q.Location, l.EnemyRC, q.Other, o.CanChoose})
end

function Activation()
	local imprisoned = obj.ChooseImprison({q.Location, l.EnemyRC, q.Other, o.CanChoose, q.Count, 1})
	obj.Store(imprisoned)
	if obj.GetNumberOf({q.Location, l.Stored}) > 0 then
		local grade = obj.GetGrade({q.Location, l.Stored})
		obj.SoulCharge(grade)
	end
end
