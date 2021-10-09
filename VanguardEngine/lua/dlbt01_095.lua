-- Sharing Happiness, Danael

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 2
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerUnits, q.Other, o.NotThis, q.Count, 1
	elseif n == 2 then
		return q.Location, l.PlayerUnits, q.Other, o.NotThis, q.Other, o.OddGrade
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnAttack, p.HasPrompt, p.WhiteWings, p.SB, 2
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsRearguard() and obj.IsBooster() then
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

function Activate(n)
	if n == 1 then
		obj.ChooseAddTempPower(1, obj.GetNumberOf(2) * 5000)
	end
	return 0
end