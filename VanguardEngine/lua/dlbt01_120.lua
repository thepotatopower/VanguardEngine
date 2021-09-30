-- Innocent Happiness

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 1
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerVC, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnOrder, p.HasPrompt
	elseif n == 2 then
		return a.OnSing, p.HasPrompt
	end
end

function CheckCondition(n)
	if n == 1 then
		return true
	elseif n == 2 then
		if obj.IsInOrderZone() then
			return true
		end
	end
	return false
end

function CanFullyResolve(n) 
	if n == 1 then
		return true
	elseif n == 2 then
		return true
	end
	return false
end

function Activate(n)
	if n == 2 then
		obj.ChooseAddTempPower(1, 5000)
	end
	return 0
end