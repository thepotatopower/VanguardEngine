-- 砂塵の穿弾 メイナード

function RegisterAbilities()
	-- on place
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnVC)
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
	if check then return obj.CanCB(2) end
	obj.CounterBlast(2)
end

function CanFullyResolve()
	return obj.Exists({q.Location, l.EnemyRC, q.Other, o.CanChoose})
end

function Activation()
	obj.Select({q.Location, l.EnemyRC, q.Other, o.CanChoose, q.Count, 1})
	if obj.GetNumberOf({q.Location, l.Selected}) > 0 then
		local column = obj.GetColumn({q.Location, l.Selected})
		obj.Retire({q.Location, l.EnemyRC, q.Column, column})
	end
end

