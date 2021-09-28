-- Harvesting Season

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 1
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.BackRow
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnOrder, p.HasPrompt
	end
end

function CheckCondition(n)
	if n == 1 then
		return true
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		if obj.GetNumberOf(1) > 0 then
			return true
		end
	end
	return false
end

function Activate(n)
	if n == 1 then
		obj.AddTempPower(1, 5000)
		obj.CannotMove()
		obj.CannotBoost()
	end
	return 0
end