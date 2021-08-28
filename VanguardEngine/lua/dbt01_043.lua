-- Hollowing Moonlit Night

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 0
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnOrder, t.Order, p.HasPrompt, p.SB, 1
	elseif n == 2 then
		return a.Cont, t.Cont, p.IsMandatory
	end
end

function CheckCondition(n)
	if n == 1 then
		return true
	elseif n == 2 then
		if obj.IsWorld() then
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

function Cost(n)
	if n == 1 then
		obj.SoulBlast(1)
	end
end

function Activate(n)
	if n == 1 then
		obj.SetWorld()
		obj.Draw(1)
	elseif n == 2 then
		if obj.OnlyWorlds() then
			if obj.NumWorlds() == 1 then
				obj.DarkNight()
			elseif obj.NumWorlds() >= 2 then
				obj.AbyssalDarkNight()
			end
		else
			obj.NoWorld()
		end
	end
	return 0
end