-- Wings With Rainbow Glow, Erimuel

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 5
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Applicable, q.Other, o.This, q.Count, 1
	elseif n == 2 then
		return q.Location, l.EnemyVC, q.Grade, 3, q.Other, o.GradeOrHigher, q.Count, 1
	elseif n == 3 then
		return q.Location, l.PlayerRC, q.Count, 0
	elseif n == 4 then
		return q.Location, l.PlayerUnits, q.Other, o.OddGrade
	elseif n == 5 then
		return q.Location, l.Selected
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.PlacedOnRC, p.HasPrompt, p.WhiteWings, p.CB, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.Exists(1) and obj.Exists(2) then
			return true
		end
	end
	return false
end

function CanFullyResolve(n) 
	if n == 1 then
		if obj.GetNumberOf(4) > 3 then
			return true
		end
	end
	return false
end

function Activate(n)
	if n == 1 then
		obj.Inject(3, q.Count, math.floor(obj.GetNumberOf(4) / 3))
		obj.Select(3)
		obj.AddTempPower(5, 5000)
		obj.AddCritical(5, 1)
	end
	return 0
end