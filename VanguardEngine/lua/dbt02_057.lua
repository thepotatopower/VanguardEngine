-- Crossrock Dragon

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 2
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.EnemyRC, q.Count, 1, q.Other, o.CanChoose
	elseif n == 2 then
		return q.Location, l.FrontRowEnemyRC
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.PlacedOnRC, t.Auto, p.HasPrompt, true, p.IsMandatory, false
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.LastPlacedOnRC() and obj.Exists(1) and obj.OpenCirclesExist(2) then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		return true
	end
	return false
end

function Cost(n)
end

function Activate(n)
	if n == 1 then
		obj.ChooseMoveEnemyRearguard(1, obj.OpenCircles(2))
	end
	return 0
end