-- Worldwide Special Live Tour!

function NumberOfAbilities()
	return 3
end

function NumberOfParams()
	return 2
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerVC, q.Count, 1
	elseif n == 2 then
		return q.Location, l.PlayerRC
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnOrder, p.HasPrompt
	elseif n == 2 then
		return a.OnACT, p.HasPrompt, p.OncePerTurn, p.CB, 2
	elseif n == 3 then
		return a.Cont, p.IsMandatory, p.Given
	end
end

function CheckCondition(n)
	if n == 1 then
		return true
	if n == 2 then
		return true
	elseif n == 2 then
		if obj.IsVanguard() then
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
	elseif n == 3 then
		return true
	end
	return false
end

function Activate(n)
	if n == 2 then
		obj.ChooseGiveAbility(1, 3)
	elseif n == 3 then
		obj.SetAbilityPower(2, 5000)
	end
	return 0
end